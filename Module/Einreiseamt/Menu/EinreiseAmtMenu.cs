﻿using System;
using System.Collections.Generic;
using System.Linq;
using Nexus.Module.Assets.Tattoo;
using Nexus.Module.Business;
using Nexus.Module.PlayerUI.Components;
using Nexus.Module.GTAN;
using Nexus.Module.Houses;
using Nexus.Module.Items;
using Nexus.Module.Menu;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.Windows;
using Nexus.Module.Tattoo;

namespace Nexus.Module.Einreiseamt
{
    public class EinreiseAmtMenuBuilder : MenuBuilder
    {
        public EinreiseAmtMenuBuilder() : base(PlayerMenu.EinreiseAmtMenu)
        {
        }

        public override Menu.Menu Build(DbPlayer iPlayer)
        {
            if (!iPlayer.IsEinreiseAmt() || !iPlayer.HasData("einreiseamtp")) return null;

            DbPlayer foundPlayer = Players.Players.Instance.FindPlayer(iPlayer.GetData("einreiseamtp"));
            if (foundPlayer == null || !foundPlayer.IsValid() || !foundPlayer.NeuEingereist()) return null;


            var menu = new Menu.Menu(Menu, "Einreise " + foundPlayer.GetName());

            menu.Add($"Schließen");
            menu.Add($"Level " + foundPlayer.Level + " | GB: " + foundPlayer.birthday[0]);
            menu.Add($"Spieler ablehnen (Permbann)");
            menu.Add($"Spieler annehmen (Perso)");

            return menu;
        }

        public override IMenuEventHandler GetEventHandler()
        {
            return new EventHandler();
        }

        private class EventHandler : IMenuEventHandler
        {
            public bool OnSelect(int index, DbPlayer iPlayer)
            {
                if (!iPlayer.IsEinreiseAmt() || !iPlayer.HasData("einreiseamtp")) return false;

                DbPlayer foundPlayer = Players.Players.Instance.FindPlayer(iPlayer.GetData("einreiseamtp"));
                if (foundPlayer == null || !foundPlayer.IsValid() || !foundPlayer.NeuEingereist()) return false;

                switch(index)
                {
                    case 2: // Ablehnen

                        foundPlayer.SendNewNotification("Ihnen wurde die Einreise nicht gestattet, Willkommen auf GLMP!");
                        foundPlayer.SendNewNotification("Bitte melden Sie sich bei Fragen im Support!");
                        iPlayer.SendNewNotification($"Sie haben {foundPlayer.GetName()} die Einreise verweigert!");

                        DBLogging.LogAdminAction(iPlayer.Player, foundPlayer.GetName(), adminLogTypes.perm, "Einreiseamt", 0, Configurations.Configuration.Instance.DevMode);
                        foundPlayer.warns[0] = 3;

                        Logging.Logger.AddToEinreiseLog(iPlayer.Id, foundPlayer.Id, false);

                        foundPlayer.Player.Kick("Einreise abgelehnt!");
                        MenuManager.DismissCurrent(iPlayer);

                        iPlayer.ResetData("einreiseamtp");

                        if (iPlayer.IsInDuty())
                        {
                            iPlayer.GiveMoney(5000);
                        }
                        else iPlayer.GiveMoney(8000);
                        return true;

                    case 3: // Annehmen
                        foundPlayer.hasPerso[0] = 1;
                        foundPlayer.Save();

                        foundPlayer.SendNewNotification("Ihnen wurde die Einreise gestattet! Viel Spaß auf Nexus, achso... denke an die Quest!");
                        iPlayer.SendNewNotification($"Sie haben {foundPlayer.GetName()} die Einreise gestattet!");
                        MenuManager.DismissCurrent(iPlayer);
                        foundPlayer.SetData("QuestAn", 1);
                        iPlayer.SetWaypoint(-218.09f, -1163.06f);
                        Logging.Logger.AddToEinreiseLog(iPlayer.Id, foundPlayer.Id, true);
                        ComponentManager.Get<TextInputBoxWindow>().Show()(iPlayer, new TextInputBoxWindowObject() { Title = "Einreiseamt-Formular", Callback = "EinreiseAmtPlayerBirthday", Message = "Geben Sie das Geburtsdatum ein : XX.XX.XXXX Beispiel : 09.12.1997 " });

                        if (iPlayer.IsInDuty())
                        {
                            iPlayer.GiveMoney(5000);
                        }
                        else iPlayer.GiveMoney(8000);
                        return true;

                    default:
                        break;
                }
                MenuManager.DismissCurrent(iPlayer);
                return false;
            }
        }
    }
}