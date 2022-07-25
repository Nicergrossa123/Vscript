using GTANetworkMethods;
using Nexus.Module.Chat;
using Nexus.Module.NSA;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Vehicles;

namespace Nexus.Module.Items.Scripts
{
    public static partial class ItemScript
    {
        public static bool Computer(DbPlayer iPlayer, ItemModel ItemData)
        {
            if (!iPlayer.IsValid()) return false;
            if (iPlayer.CanNSADuty())
            {
                Module.Menu.MenuManager.Instance.Build(Nexus.Module.Menu.PlayerMenu.NSAComputerMenu, iPlayer).Show(iPlayer);
                return true;
            }
            if (iPlayer.IsInDuty() && iPlayer.TeamId == (uint)teams.TEAM_FIB)
            {
                Module.Menu.MenuManager.Instance.Build(Nexus.Module.Menu.PlayerMenu.NSAComputerMenu, iPlayer).Show(iPlayer);
                return true;
            }
            if ((iPlayer.IsInDuty() && iPlayer.Team.Id == (int)teams.TEAM_GOV))
            {
                Module.Menu.MenuManager.Instance.Build(Nexus.Module.Menu.PlayerMenu.GOVComputerMenu, iPlayer).Show(iPlayer);
                return true;
            }
            return false;
        }
    }
}