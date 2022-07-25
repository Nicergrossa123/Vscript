using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nexus.Handler;
using Nexus.Module.PlayerUI.Components;
using Nexus.Module.Doors;
using Nexus.Module.Menu;
using Nexus.Module.NSA.Observation;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.JumpPoints;
using Nexus.Module.Players.Windows;
using Nexus.Module.Telefon.App;
using Nexus.Module.Vehicles;

namespace Nexus.Module.NSA.Menu
{
    public class NSADoorUsedsMenuBuilder : MenuBuilder
    {
        public NSADoorUsedsMenuBuilder() : base(PlayerMenu.NSADoorUsedsMenu)
        {

        }

        public override Module.Menu.Menu Build(DbPlayer p_DbPlayer)
        {
            var l_Menu = new Module.Menu.Menu(Menu, "NSA Keycard Nutzungen");
            l_Menu.Add($"Schließen");

            if (p_DbPlayer.TryData("doorId", out uint doorId))
            {
                Door door = DoorModule.Instance.Get(doorId);
                if (door == null) return null;
                if (door.LastUseds.Count > 0)
                {
                    foreach (LastUsedFrom lastUsed in door.LastUseds)
                    {
                        l_Menu.Add($"{lastUsed.Name} - {lastUsed.DateTime.ToShortTimeString()} - {(lastUsed.Opened ? "geöffnet" : "geschlossen")}");
                    }
                }
            }

            if (p_DbPlayer.TryData("jumpPointId", out int jpid))
            {
                JumpPoint jumpPoint = JumpPointModule.Instance.Get(jpid);
                if (jumpPoint == null) return null;
                if (jumpPoint.LastUseds.Count > 0)
                {
                    foreach (LastUsedFrom lastUsed in jumpPoint.LastUseds)
                    {
                        l_Menu.Add($"{lastUsed.Name} - {lastUsed.DateTime.ToShortTimeString()} - {(lastUsed.Opened ? "geöffnet" : "geschlossen")}");
                    }
                }
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
                MenuManager.DismissCurrent(iPlayer);
                return true;
            }
        }
    }
}
