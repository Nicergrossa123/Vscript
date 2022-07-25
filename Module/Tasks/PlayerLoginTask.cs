using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GTANetworkAPI;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Nexus.Handler;
using Nexus.Module.PlayerUI.Components;
using Nexus.Module.PlayerUI.Windows;
using Nexus.Module.Clothes;
using Nexus.Module.Configurations;
using Nexus.Module.Helper;
using Nexus.Module.Logging;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.Windows;
using Nexus.Module.Time;

namespace Nexus.Module.Tasks
{
    public class PlayerLoginTask : SqlResultTask
    {
        private readonly Player player;

        public PlayerLoginTask(Player player)
        {
            this.player = player;
        }

        public override string GetQuery()
        {
            return $"SELECT * FROM `player` WHERE `Name` = '{MySqlHelper.EscapeString(player.Name)}' LIMIT 1;";
        }

        public override void OnFinished(MySqlDataReader reader)
        {
            try
            {


                if (reader.HasRows)
                {
                    DbPlayer iPlayer = null;
                    while (reader.Read())
                    {
                        if (player == null) return;
                        //Bei Warn hau wech
                        if (reader.GetInt32("warns") >= 3)
                        {
                            player.TriggerEvent("freezePlayer", true);
                            //player.Freeze(true);
                            player.CreateUserDialog(Dialogs.menu_register, "banwindow");

                            PlayerLoginDataValidationModule.SyncUserBanToForum(reader.GetInt32("forumid"));

                            player.SendNotification($"Dein NEXUSRP (IC-)Account wurde gesperrt. Melde dich im Teamspeak!");
                            player.Kick();
                            return;
                        }

                        if (!PlayerLoginDataValidationModule.HasValidForumAccount(reader.GetInt32("forumid")))
                        {
                            player.TriggerEvent("freezePlayer", true);

                            player.CreateUserDialog(Dialogs.menu_register, "banwindow");

                            player.Kick("Dein Forumaccount ist nicht für das Spiel freigeschaltet!");
                            return;
                        }


                        // Check Timeban
                        //     if (reader.GetInt32("timeban") != 0 && reader.GetInt32("timeban") > DateTime.Now.GetTimestamp())
                        //   {
                        //       player.SendNotification("Ban aktiv");
                        //      player.Kick("Ban aktiv");
                        //      return;
                        //  }

                        iPlayer = Players.Players.Instance.Load(reader, player);


                        //     iPlayer.Freezed = false;
                        iPlayer.watchDialog = 0;
                        iPlayer.Firstspawn = false;
                        iPlayer.PassAttempts = 0;
                        iPlayer.TempWanteds = 0;
                        iPlayer.PlayerPet = null;

                        iPlayer.adminObject = null;
                        iPlayer.adminObjectSpeed = 0.5f;

                        iPlayer.AccountStatus = AccountStatus.Registered;

                        iPlayer.Character = ClothModule.Instance.LoadCharacter(iPlayer);

                        VehicleKeyHandler.Instance.LoadPlayerVehicleKeys(iPlayer);

                        //           iPlayer.SetPlayerCurrentJobSkill();
                        //iPlayer.ClearChat();

                        // Check Socialban
                        if (SocialBanHandler.Instance.IsPlayerSocialBanned(iPlayer.Player))
                        {
                            player.SendNotification("Bitte melde dich beim Support im Teamspeak (Social-Ban)");
                            player.Kick();
                            return;
                        }

                        // Check Social Name
                        //if (!Configurations.Configuration.Instance.Ptr && iPlayer.SocialClubName != "" && iPlayer.SocialClubName != iPlayer.Player.SocialClubName)
                        //   {
                        //    //DBLogging.LogAcpAdminAction("System", player.Name, adminLogTypes.perm, $"Social-Club-Name-Changed DB - {iPlayer.SocialClubName} - CLIENT - {iPlayer.Player.SocialClubName}");
                        //    iPlayer.Player.SendNotification("Bitte melde dich beim Support im Teamspeak (Social-Name-Changed)");
                        //    iPlayer.Player.Kick("Bitte melde dich beim Support im Teamspeak (Social-Name-Changed)");
                        //      return;
                        //   }


                        if (Players.Players.Instance.players.ToList().Count >= Configuration.Instance.MaxPlayers)
                        {
                            iPlayer.Player.SendNotification($"Server voll! ({Configuration.Instance.MaxPlayers.ToString()})");
                            iPlayer.Player.Kick("Server voll");
                        }

                        //            player.FreezePosition = true;

                        if (iPlayer == null) return;
                        player.Name = reader.GetString("Name");
                        player.TriggerEvent("setPlayerHealthRechargeMultiplier");
                        ComponentManager.Get<LoginWindow>().Show()(iPlayer);

                        if (Configuration.Instance.IsUpdateModeOn)
                        {
                            new LoginWindow().TriggerEvent(iPlayer.Player, "status", "Der Server befindet sich derzeit im Update Modus!");
                            if (iPlayer.Rank.Id < 1) iPlayer.Kick();
                        }

                    }
                }
                else
                {
                    TextInputBoxWindowObject textInputBoxObject = new TextInputBoxWindowObject
                    {
                        Title = "Anmeldeformular",
                        Message = "Gebe bitte deinen Benutzernamen ein (Beispiel: Vorname_Nachname). Falls du noch nicht registriert bist, wirst du automatisch registriert.",
                        Callback = "registerUser"
                    };

                    player.OpenTextInputBox(textInputBoxObject);
                    //GTANetworkAPI.NAPI.Task.Run(() => ComponentManager.Get<TextInputBoxWindow>().Show()(iPlayer, new TextInputBoxWindowObject() { Title = "Kennzeichen", Callback = "SetKennzeichen", Message = "Gib ein Kennzeichen ein (8 Zeichen)" }));
                    //   Random id = new Random();

                    //    Random num = new Random();
                    ////   MySQLHandler.ExecuteAsync($"INSERT INTO `player` (`id`, `Name`, `handy`, `rankId`, `einwanderung`, `pos_x`, `pos_y`, `pos_z`, `SCName`, `Pass`) VALUES ('{id.Next(50, 99999)}', '{player.Name}', '{num.Next(50, 99999)}', '6', '1', '133', '-766', '243', '{player.SocialClubName}', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3');");
                    //    player.Kick("Account Erstellt, bitte neu einloggen!");

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}