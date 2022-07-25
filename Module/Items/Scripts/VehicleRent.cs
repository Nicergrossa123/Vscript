using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GTANetworkAPI;
using Nexus.Module.Chat;
using Nexus.Module.Menu;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.PlayerAnimations;

namespace Nexus.Module.Items.Scripts
{
    public static partial class ItemScript
    {
        public static bool vehiclerent(DbPlayer iPlayer, ItemModel ItemData)
        {
            if (!iPlayer.CanInteract())
            {
                return false;
            }
            
            MenuManager.Instance.Build(PlayerMenu.VehicleRentMenu, iPlayer).Show(iPlayer);
            return false;
        }

        public static bool vehiclerentview(DbPlayer iPlayer, ItemModel ItemData, Item item)
        {
            if (!iPlayer.CanInteract())
            {
                return false;
            }

            if (item.Data != null && item.Data.ContainsKey("info"))
            {
                iPlayer.SendNewNotification($"KFZ-Mietvertrag: {item.Data["info"]}", PlayerNotification.NotificationType.STANDARD, "", 12000);
            }
            return false;
        }
    }
}