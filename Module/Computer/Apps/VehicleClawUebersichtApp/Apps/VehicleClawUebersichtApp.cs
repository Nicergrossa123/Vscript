using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using Nexus.Module.PlayerUI.Apps;
using Nexus.Module.Computer.Apps.KennzeichenUebersichtApp;
using Nexus.Module.LeitstellenPhone;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;

namespace Nexus.Module.Computer.Apps.VehicleClawUebersichtApp.Apps
{
    public class VehicleClawUebersichtApp : SimpleApp
    {
        public VehicleClawUebersichtApp() : base("VehicleClawUebersichtApp") { }
        public enum SearchType
        {
            PLAYERNAME = 0,
            VEHICLEID = 1
        }


        [RemoteEvent]
        public async Task requestVehicleClawOverviewByPlayerName(Player Player, String playerName)
        {
            if (!MySQLHandler.IsValidNoSQLi(Player, playerName)) return;
            await HandleVehicleClawOverview(Player, playerName, SearchType.PLAYERNAME);
        }

        [RemoteEvent]
        public async Task requestVehicleClawOverviewByVehicleId(Player Player, int vehicleId)
        {
            await HandleVehicleClawOverview(Player, vehicleId.ToString(), SearchType.VEHICLEID);

        }


        private async Task HandleVehicleClawOverview(Player p_Player, String information, SearchType type)
        {
            DbPlayer p_DbPlayer = p_Player.GetPlayer();
            if (p_DbPlayer == null || !p_DbPlayer.IsValid())
                return;

            if (LeitstellenPhoneModule.Instance.GetByAcceptor(p_DbPlayer) == null)
            {
                p_DbPlayer.SendNewNotification("Sie müssen als Leitstelle angemeldet sein", PlayerNotification.NotificationType.ERROR);
                return;
            }

            var l_Overview = VehicleClawUebersichtFunctions.GetVehicleClawByIdOrName(p_DbPlayer, type, information);
            TriggerEvent(p_Player, "responseVehicleClawOverview", NAPI.Util.ToJson(l_Overview));
        }

    }
}
