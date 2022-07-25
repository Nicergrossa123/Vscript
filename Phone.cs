using Newtonsoft.Json;
using Nexus.Module.GTAN;
using Nexus.Module.Players.Db;

namespace Nexus
{
    public class PlayerPhoneData
    {
        public int Credit { get; set; }
        public uint Number { get; set; }
    }

    public static class Phone
    {
        public static void SetPlayerPhoneData(DbPlayer iPlayer)
        {
            var data = new PlayerPhoneData {Credit = iPlayer.guthaben[0], Number = iPlayer.handy[0]};
            iPlayer.Player.TriggerEvent("RESPONSE_PHONE_SETTINGS", JsonConvert.SerializeObject(data));
        }
    }
}