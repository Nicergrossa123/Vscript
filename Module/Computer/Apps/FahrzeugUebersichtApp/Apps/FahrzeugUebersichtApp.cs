using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nexus.Module.PlayerUI.Apps;
using Nexus.Module.Computer.Apps.FahrzeugUebersichtApp;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;

namespace Nexus.Module.Computer.Apps.FahrzeuguebersichtApp.Apps
{
    public class FahrzeugUebersichtApp : SimpleApp
    {
        public FahrzeugUebersichtApp() : base("FahrzeugUebersichtApp") { }


        public enum OverviewCategory
        {
            OWN=0,
            KEY=1,
            BUSINESS=2,
            RENT = 3
        }



        [RemoteEvent]
        public void requestVehicleOverviewByCategory(Player Player, int id)
        {
            DbPlayer p_DbPlayer = Player.GetPlayer();
            if (p_DbPlayer == null || !p_DbPlayer.IsValid())
                return;

            OverviewCategory l_Category = (OverviewCategory)id;
            var l_Overview = FahrzeugUebersichtFunctions.GetOverviewVehiclesForPlayerByCategory(p_DbPlayer, l_Category);
            TriggerEvent(Player, "responseVehicleOverview", NAPI.Util.ToJson(l_Overview));
        }
    }
}
