using System.Threading.Tasks;
using GTANetworkAPI;
using Nexus.Module.PlayerUI.Components;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.PlayerAnimations;
using Nexus.Module.Players.Windows;

namespace Nexus.Module.Items.Scripts
{
    public static partial class ItemScript
    {
        public static bool Digging(DbPlayer iPlayer)
        {
            if (iPlayer.Player.IsInVehicle) return false;
            return false;

            ComponentManager.Get<TextInputBoxWindow>().Show()(iPlayer, new TextInputBoxWindowObject() { Title = "Gold-Digger", Callback = "DigTo", Message = "Gib einen Namen zudem du drich graben willst ein" });
            return true;
        }
    }
}