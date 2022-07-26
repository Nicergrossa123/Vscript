﻿using System;
using System.Collections.Generic;
using Nexus.Module.Banks;
using Nexus.Module.Banks.BankHistory;
using Nexus.Module.Banks.Windows;
using Nexus.Module.PlayerUI.Components;
using Nexus.Module.Menu;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;

namespace Nexus.Module.Boerse.Menu
{
    public class ManageDepotMenu : MenuBuilder
    {
        public ManageDepotMenu() : base(PlayerMenu.ManageDepotMenu)
        {
        }

        public override Module.Menu.Menu Build(DbPlayer iPlayer)
        {
            Module.Menu.Menu menu = new Module.Menu.Menu(PlayerMenu.ManageDepotMenu, "Depot-Management", "");
            menu.Add("Schließen", "");
            menu.Add("Depot erstellen", "");
            
            if (iPlayer.HasDepot())
                menu.Add("Depot verwalten", "");

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
                switch (index)
                {
                    case 1: // Depot erstellen
                        if (iPlayer.HasDepot())
                            iPlayer.SendNewNotification("Du hast bereits ein Depot!", PlayerNotification.NotificationType.ADMIN, "Fehler!", 5000);
                        else
                            iPlayer.CreateDepot();
                        break;
                    case 2: // Depot ein- und auszahlen
                        if (!iPlayer.HasDepot())
                            break;
                        
                        ComponentManager.Get<BankWindow>().Show()(iPlayer, "Aktien-Depot", iPlayer.GetName(), iPlayer.money[0], (int)iPlayer.Depot.Amount, 0, new List<BankHistory>());
                        break;
                    default: // Wird aufgerufen, wenn Schließen ausgewählt wurde
                        break;
                }

                MenuManager.DismissCurrent(iPlayer);
                return true;
            }
        }
    }
}