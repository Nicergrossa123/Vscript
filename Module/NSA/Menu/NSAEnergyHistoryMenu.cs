﻿using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nexus.Handler;
using Nexus.Module.PlayerUI.Components;
using Nexus.Module.Menu;
using Nexus.Module.NSA.Observation;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.Windows;
using Nexus.Module.Telefon.App;

namespace Nexus.Module.NSA.Menu
{
    public class NSAEnergyHistoryMenuBuilder : MenuBuilder
    {
        public NSAEnergyHistoryMenuBuilder() : base(PlayerMenu.NSAEnergyHistory)
        {

        }

        public override Module.Menu.Menu Build(DbPlayer p_DbPlayer)
        {
            var l_Menu = new Module.Menu.Menu(Menu, "IAA Energiemeldung History");
            l_Menu.Add($"Schließen");

            foreach (TransactionHistoryObject transactionHistoryObject in NSAModule.TransactionHistory.ToList().Where(t => t.TransactionType == TransactionType.ENERGY))
            {
                l_Menu.Add($"{transactionHistoryObject.Description} - {transactionHistoryObject.Added.ToShortTimeString()}");
            }
            
            return l_Menu;
        }

        public override IMenuEventHandler GetEventHandler()
        {
            return new EventHandler();
        }

        private class EventHandler : IMenuEventHandler
        {
            public bool OnSelect(int index, DbPlayer iPlayer)
            {
                if (index == 0)
                {
                    iPlayer.Player.TriggerEvent("removeiaaBlip");
                    MenuManager.DismissCurrent(iPlayer);
                    return true;
                }
                else
                {
                    int idx = 1;
                    foreach (TransactionHistoryObject transactionHistoryObject in NSAModule.TransactionHistory.ToList().Where(t => t.TransactionType == TransactionType.ENERGY))
                    {
                        if (idx == index)
                        {
                            iPlayer.Player.TriggerEvent("setPlayerGpsMarker", transactionHistoryObject.Position.X, transactionHistoryObject.Position.Y);
                            return false;
                        }
                        idx++;
                    }
                }
                MenuManager.DismissCurrent(iPlayer);
                return true;
            }
        }
    }
}
