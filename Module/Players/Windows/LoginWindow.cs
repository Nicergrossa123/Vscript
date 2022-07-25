using System;
using Nexus.Module.Players.Db;
using Newtonsoft.Json;
using GTANetworkAPI;
using Nexus.Module.Chat;
using Nexus.Module.PlayerUI.Windows;
using Nexus.Module.Logging;
using Nexus.Module.Players.Events;
using System.Threading.Tasks;
using Nexus.Module.Configurations;
using Nexus.Module.Customization;
using System.Net;
using Nexus.Module.Helper;
using Nexus.Module.Time;
using System.Linq;
using Nexus.Module.Anticheat;

namespace Nexus.Module.Players.Windows
{
    public class LoginWindow : Window<Func<DbPlayer, bool>>
    {
        private class ShowEvent : Event
        {
            [JsonProperty(PropertyName = "name")] private string Name { get; }
            [JsonProperty(PropertyName = "rank")] private uint Rank { get; }

            public ShowEvent(DbPlayer dbPlayer, string name, uint rank) : base(dbPlayer)
            {
                Name = name;
                Rank = rank;
            }
        }

        public LoginWindow() : base("Login")
        {
        }

        public override Func<DbPlayer, bool> Show()
        {
            return player => OnShow(new ShowEvent(player, player.GetName(), player.RankId));
        }
        
        [RemoteEvent]
        public void PlayerLogin(Player player, string password)
        {
            Main.m_AsyncThread.AddToAsyncThread(new Task(() =>
            {
                var dbPlayer = player.GetPlayer();
                if (dbPlayer == null) return;


                if (dbPlayer.AccountStatus != AccountStatus.Registered)
                {
                    dbPlayer.SendNewNotification("Sie sind bereits eingeloggt!");
                    TriggerEvent(player, "status", "successfully");
                    return;
                }

                var pass = password;
                var pass2 = dbPlayer.Password;

                if (dbPlayer.Password == "")
                {
                    // MySQLHandler.ExecuteAsync($"UPDATE player SET Pass = {password} WHERE id = '{dbPlayer.Id}'");
                    Main.SetNewPassword(dbPlayer, password);
                    string message = string.Format("Passwort gesetzt, Bitte neu einloggen!");
                    //dbPlayer.SendNewNotification(message, title:"SERVER", notificationType:PlayerNotification.NotificationType.SERVER);
                    TriggerEvent(player, "status", message);
                    return;
                }
                Console.WriteLine(pass, pass2);
                if (pass == pass2)
                {
                    Logger.SaveLoginAttempt(dbPlayer.Id, dbPlayer.Player.SocialClubName, dbPlayer.Player.Address, 1);

                    try
                    {
                        // Set Data that Player is Connected
                        dbPlayer.Player.SetData("Connected", true);

                        dbPlayer.AccountStatus = AccountStatus.LoggedIn;

                        dbPlayer.SetACLogin();

                        //Set online
                        var query =
                            $"UPDATE `player` SET `Online` = '{1}', LastLogin = '{DateTime.Now.GetTimestamp()}' WHERE `id` = '{dbPlayer.Id}';";
                        MySQLHandler.ExecuteAsync(query);

                        dbPlayer.Player.ResetData("loginStatusCheck");

                        TriggerEvent(player, "status", "successfully");

                        player.SetSharedData("AC_Status", true);

                        // send phone data
                        Nexus.Phone.SetPlayerPhoneData(dbPlayer);

                        var duplicates = NAPI.Pools.GetAllPlayers().FindAll(p => p.Name == player.Name && p != player);

                        try
                        {
                            foreach (var itr in Players.Instance.players.Where(p => p.Value.GetName() == player.Name && p.Value.Player != player))
                            {
                                Players.Instance.players.TryRemove(itr.Key, out DbPlayer tmpDbPlayer);
                            }
                        }
                        catch (Exception e)
                        {
                            Logging.Logger.Crash(e);
                        }

                        if (duplicates.Count > 0)
                        {
                            try
                            {
                                foreach (var duplicate in duplicates)
                                {
                                    Logger.Debug($"Duplicated Player {duplicate.Name} deleted");

                                    duplicate.Delete();

                                    duplicate.SendNotification("Duplicated Player");
                                    duplicate.Kick();
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.Crash(ex);
                                // ignored
                            }
                        }

                        dbPlayer.Firstspawn = true;
                        // Character Sync
                        NAPI.Task.Run(() =>
                        {
                            dbPlayer.ApplyCharacter(true);
                            dbPlayer.ApplyPlayerHealth();
                            dbPlayer.Player.TriggerEvent("setPlayerHealthRechargeMultiplier");
                        }, 3000);
                        PlayerSpawn.OnPlayerSpawn(player);
                        //dbPlayer.SendNewNotification($"Bitte verbinde auf folgenden Teamspeak für das Ingame-Voice: testvoice.gvmp.de:10000", PlayerNotification.NotificationType.ADMIN, "Unser Voice-Server ist umgezogen!", 60000);
                        //dbPlayer.SendNewNotification($"Du hast 2 Minuten Zeit dich im anderen Voice einzufinden. Sonst wirst du vom Server gekickt!", PlayerNotification.NotificationType.ADMIN, "ACHTUNG!", 120000);
                        dbPlayer.SetData("login_time", DateTime.Now);
                    }
                    catch (Exception e)
                    {
                        Logger.Crash(e);
                    }
                }
                else
                {
                    Logger.SaveLoginAttempt(dbPlayer.Id, dbPlayer.Player.SocialClubName, dbPlayer.Player.Address, 0);
                    dbPlayer.PassAttempts += 1;

                    if (dbPlayer.PassAttempts >= 3)
                    {
                        //dbPlayer.SendNewNotification("Sie haben ein falsches Passwort 3x eingegeben, Sicherheitskick.", title:"SERVER", notificationType:PlayerNotification.NotificationType.SERVER);
                        TriggerEvent(player, "status", "Passwort wurde 3x falsch eingegeben. Sicherheitskick");
                        player.Kick("Falsches Passwort (3x)");
                        return;
                    }

                    string message = string.Format(

                        "Falsches Passwort ({0}/3)",
                        dbPlayer.PassAttempts);
                    //dbPlayer.SendNewNotification(message, title:"SERVER", notificationType:PlayerNotification.NotificationType.SERVER);
                    TriggerEvent(player, "status", message);
                }
            }));
        }
    }
}