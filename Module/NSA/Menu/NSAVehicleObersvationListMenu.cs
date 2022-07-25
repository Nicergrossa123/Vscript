using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nexus.Handler;
using Nexus.Module.PlayerUI.Components;
using Nexus.Module.Menu;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.Windows;

namespace Nexus.Module.NSA.Menu
{
    public class NSAVehicleObersvationListMenuBuilder : MenuBuilder
    {
        public NSAVehicleObersvationListMenuBuilder() : base(PlayerMenu.NSAVehicleObersvationListMenu)
        {

        }

        public override Module.Menu.Menu Build(DbPlayer p_DbPlayer)
        {
            if (!p_DbPlayer.HasData("nsa_target_player_id")) return null;

            DbPlayer l_Target = Players.Players.Instance.FindPlayerById(p_DbPlayer.GetData("nsa_target_player_id"));
            if (l_Target == null || !l_Target.IsValid()) return null;

            var l_Menu = new Module.Menu.Menu(Menu, "Fahrzeug Schlüssel");
            l_Menu.Add($"Schließen");

            foreach (KeyValuePair<uint, string> kvp in l_Target.VehicleKeys.ToList())
            {
                l_Menu.Add($"({kvp.Key}) {kvp.Value}");
            }

            foreach (SxVehicle sxVehicle in VehicleHandler.Instance.GetPlayerVehicles(l_Target.Id).ToList())
            {
                l_Menu.Add($"({sxVehicle.databaseId}) {sxVehicle.GetName()}");
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
