using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nexus.Module.PlayerUI.Apps;
using Nexus.Module.Events.CWS;
using Nexus.Module.Players.Db;

namespace Nexus.Module.Players.Phone.Apps
{
    public class ProfileApp : SimpleApp
    {
        public ProfileApp() : base("ProfileApp")
        {
        }

        [RemoteEvent]
        public void requestSpecialProfilData(Player player)
        {
            DbPlayer dbPlayer = player.GetPlayer();

            if (dbPlayer == null || !dbPlayer.IsValid()) return;

            Dictionary<string, int> CWSResults = new Dictionary<string, int>();

            foreach(KeyValuePair<uint, PlayerCWS> kvp in dbPlayer.CWS)
            {
                CWS cws = CWSModule.Instance.GetAll().Values.Where(cc => cc.Id == kvp.Key).FirstOrDefault();
                if (cws == null) continue;
                CWSResults.Add(cws.Name, kvp.Value.Value);
            }
            
            TriggerEvent(player, "responseSpecialProfilData", NAPI.Util.ToJson(CWSResults));
        }
    }
}
