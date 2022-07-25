using GTANetworkAPI;
using Nexus.Module.PlayerUI.Apps;
using Nexus.Module.Injury;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.Phone;

namespace Nexus.Module.Telefon.App
{
    public class TelefonApp : SimpleApp
    {
        public TelefonApp() : base("Telefon")
        {
        }

        public DbPlayer GetPlayerByPhoneNumber(int p_PhoneNumber)
        {
            foreach (var l_Player in Players.Players.Instance.GetValidPlayers())
            {
                if ((int)l_Player.handy[0] != p_PhoneNumber)
                    continue;

                return l_Player;
            }

            return null;
        }
    }
}