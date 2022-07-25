/*using GTANetworkAPI;
using Nexus.Module.Logging;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.Events;
using Nexus.Module.Players.Windows;
using Nexus.Module.Tasks;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Nexus.Module.PlayerUI.Components;

namespace Nexus
{
    public class CreateAccount : Script
    {
        [RemoteEvent]
        public static void registerUser(Player c, string username)
        {
            try
            {
                string query = $"SELECT MAX(id) as maxId FROM player";
            using (var conn = new MySqlConnection(Module.Configurations.Configuration.Instance.GetMySqlConnection()))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @query;
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            bool flag = false;
            foreach (char c2 in username)
            {
                if (c2 == '_')
                {
                    flag = true;
                }
            }

            if (username.Length > 32)
                flag = false;

            if (username != username.RemoveSpecialCharacters())
                flag = false;
              int count = username.Count(f => f == '_');
              if (!Regex.IsMatch(username, @"^[a-zA-Z_-]+$") || count != 1)
              {
             c.SendNotification("Bitte gib einen Namen in dem Format Max_Mustermann an.", true);
             return;
            }
            if (!flag)
            {
                c.SendNotification("Der Name darf nur aus einem Vor- und Nachnamen bestehen und nur 32 Zeichen lang sein. Beispiel: Aspect_Rich", true);
                TextInputBoxWindowObject textInputBoxObject = new TextInputBoxWindowObject
                {
                    Title = "Anmeldeformular",
                    Message = "Gebe bitte deinen Benutzernamen ein (Beispiel: Vorname_Nachname). Falls du noch nicht registriert bist, wirst du automatisch registriert.",
                    Callback = "registerUser"
                };

                c.OpenTextInputBox(textInputBoxObject);
            }
            else
            {
                                //Console.WriteLine("Account Erstellt: " + username);
                string account = Nexus.Main.getAccounts(username);
                if (account != "")
                 {
                 c.SendNotification("Der Name ist bereits vergeben", true);
                   return;
                 }
                else
                 { 
                c.Name = username;

                int id = reader.GetInt32("maxId") + 1;
                if (c.SocialClubName == "E6IIRA2")
                {
                                    MySQLHandler.ExecuteAsync($"INSERT INTO `player` (`id`, `forumid`, `Name`, `handy`, `rankId`, `einwanderung`, `pos_x`, `pos_y`, `pos_z`, `SCName`, `Pass`) VALUES ('{id}', '{id}', '{username}', '{id + 1275}', '6', '1', '-1042', '-2745', '22', '{c.SocialClubName}', '');");
                                }
                else
                {
                MySQLHandler.ExecuteAsync($"INSERT INTO `player` (`id`, `forumid`, `Name`, `handy`, `rankId`, `einwanderung`, `pos_x`, `pos_y`, `pos_z`, `SCName`, `Pass`) VALUES ('{id}', '{id}', '{username}', '{id + 1275}', '0', '1', '-1042', '-2745', '22', '{c.SocialClubName}', '');");
                }
                c.SendNotification("Dein Account wurde erfolgreich erstellt. Bitte Spiel neustarten!");
                c.Kick();
                // c.OpenLoginWindow(username);
                //  c.Name = username;
                //  WebhookSender.SendMessage("TEXTINPUTBOX", "" + c.Name + "" + username + " - LOGIN", Webhooks.hauslagerlogs, "Hauslager");
            }
                            }
                        }
                    }
                    }
                }
            }
             catch (Exception e)
            {
                Logger.Crash(e);
                return;
        }
            }


        [RemoteEvent]
        public void setgeburtstag(Player player, string gb)
        {
            try
            {

            
            var dbPlayer = player.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid()) return;


            if (!DateTime.TryParseExact(gb, new string[] { "dd.mm.yyyy" },
                System.Globalization.CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime dt))
            {
                dbPlayer.SendNewNotification("Geburtsdatum muss im Format TAG.MONAT.JAHR eingegeben werden : 09.12.1997");
                ComponentManager.Get<TextInputBoxWindow>().Show()(dbPlayer, new TextInputBoxWindowObject() { Title = "Einreiseamt-Formular", Callback = "setgeburtstag", Message = "Geben Sie ihr Geburtsdatum ein : XX.XX.XXXX Beispiel : 09.12.1997 " });
            }
            else
            {
                MySQLHandler.ExecuteAsync($"UPDATE player SET birthday = '{gb}' WHERE id = '{dbPlayer.Id}';");
                dbPlayer.birthday[0] = gb;
                dbPlayer.hasPerso[0] = 1;
                dbPlayer.Save();

            }


        }
            catch (Exception e)
            {
                Logger.Crash(e);
                return;
            }
        }
    }
}*/
