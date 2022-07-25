using System.Threading.Tasks;
using GTANetworkAPI;
using Nexus.Module.Chat;
using Nexus.Module.PlayerUI.Components;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.PlayerAnimations;
using Nexus.Module.Players.Windows;

namespace Nexus.Module.Items.Scripts
{
    public static partial class ItemScript
    {
        public static async Task<bool> VehicleNote(DbPlayer iPlayer, Item ItemData)
        {
            if (!iPlayer.Player.IsInVehicle) return false;
            await Task.Delay(1);
            NAPI.Task.Run(() => ComponentManager.Get<TextInputBoxWindow>().Show()(
                iPlayer, new TextInputBoxWindowObject() { Title = "Notiz", Callback = "SetVehicleNote", Message = "Gib eine Notiz ein (15 Zeichen) Nur Buchstaben und Zahlen" }));
            return false;
        }
    }
}