using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using Nexus.Module.PlayerUI.Apps;
using Nexus.Module.Computer.Apps.FahrzeugUebersichtApp;
using Nexus.Module.LeitstellenPhone;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;

namespace Nexus.Module.Computer.Apps.KennzeichenUebersichtApp.Apps
{
    public class KennzeichenUebersichtApp : SimpleApp
    {
        public KennzeichenUebersichtApp() : base("KennzeichenUebersichtApp") { }

        public enum SearchType
        {
            PLATE = 0,
            VEHICLEID = 1
        }

        [RemoteEvent]
        public async Task requestVehicleOverviewByPlate(Player Player, String plate)
        {
            if (!MySQLHandler.IsValidNoSQLi(Player, plate)) return;

            await HandleVehicleOverview(Player, plate, SearchType.PLATE);
        }

        [RemoteEvent]
        public async Task requestVehicleOverviewByVehicleId(Player Player, int vehicleId)
        {
            await HandleVehicleOverview(Player, vehicleId.ToString(), SearchType.VEHICLEID);
            
        }


        private async Task HandleVehicleOverview(Player p_Player, String information, SearchType type)
        {
            DbPlayer p_DbPlayer = p_Player.GetPlayer();
            if (p_DbPlayer == null || !p_DbPlayer.IsValid())
                return;

            if (LeitstellenPhoneModule.Instance.GetByAcceptor(p_DbPlayer) == null)
            {
                p_DbPlayer.SendNewNotification("Sie müssen als Leitstelle angemeldet sein", PlayerNotification.NotificationType.ERROR);
                return;
            }

            var l_Overview = KennzeichenUebersichtFunctions.GetVehicleInfoByPlateOrId(p_DbPlayer, type, information);
            TriggerEvent(p_Player, "responsePlateOverview", NAPI.Util.ToJson(l_Overview));
        }


    }
}
