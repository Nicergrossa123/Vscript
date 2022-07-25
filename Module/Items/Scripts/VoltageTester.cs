using System;
using System.Linq;
using System.Threading.Tasks;
using GTANetworkAPI;
using Nexus.Handler;
using Nexus.Module.Chat;
using Nexus.Module.Events.Halloween;
using Nexus.Module.Gangwar;
using Nexus.Module.Houses;
using Nexus.Module.Laboratories;
using Nexus.Module.Meth;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.PlayerAnimations;
using Nexus.Module.Vehicles;

namespace Nexus.Module.Items.Scripts
{
    public static partial class ItemScript
    {
        public static async Task<bool> VoltageTest(DbPlayer iPlayer, ItemModel ItemData)
        {
            if (iPlayer.Player.IsInVehicle || iPlayer.TeamId != (int)teams.TEAM_FIB) return false;

            HousesVoltage housesVoltage = HousesVoltageModule.Instance.GetAll().Values.ToList().Where(hv => hv.Position.DistanceTo(iPlayer.Player.Position) < 5.0f).FirstOrDefault();

            if (housesVoltage == null) return false;
            
            Module.Menu.MenuManager.Instance.Build(Nexus.Module.Menu.PlayerMenu.VoltageMenu, iPlayer).Show(iPlayer);
            return true;
        }
        
    }
}