using Nexus.Module.PlayerUI.Apps;
using GTANetworkAPI;
using Nexus.Module.Players;
using Nexus.Module.Players.Phone.Apps;

namespace Nexus.Module.Teams.Apps
{
    public class TeamEditApp : SimpleApp
    {
        public TeamEditApp() : base("TeamEditApp")
        {
        }

        [RemoteEvent]
        public void leaveTeam(Player player)
        {
            var dbPlayer = player.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid()) return;

            dbPlayer.SetTeam((uint) TeamList.Zivilist);
        }
    }
}