using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using Nexus.Module.PlayerUI.Apps;
using Nexus.Module.Logging;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;

namespace Nexus.Module.Computer.Apps.VehicleTaxApp.Apps
{
    public class VehicleTaxApp : SimpleApp
    {
        public VehicleTaxApp() : base("VehicleTaxApp") { }

        [RemoteEvent]
        public void requestVehicleTaxByModel(Player Player, String searchString)
        {
            DbPlayer dbPlayer = Player.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid()) return;
            if (!MySQLHandler.IsValidNoSQLi(dbPlayer, searchString)) return;

            var overview = VehicleTaxFunctions.GetVehicleTaxOverviews(dbPlayer, searchString);
            TriggerEvent(Player, "responseVehicleTax", NAPI.Util.ToJson(overview));
        }
    }
}
