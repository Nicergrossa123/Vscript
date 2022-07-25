using System;
using System.Linq;
using System.Threading.Tasks;
using GTANetworkAPI;
using Nexus.Module.Chat;
using Nexus.Module.Events.Halloween;
using Nexus.Module.Players;

using Nexus.Module.Players.Db;
using Nexus.Module.Weapons.Data;

namespace Nexus.Module.Items.Scripts
{
    public static partial class ItemScript
    {
        public static async Task<bool> BlackMoneyPack(DbPlayer iPlayer, ItemModel ItemData, Item item, int slot)
        {
            if (iPlayer.Player.IsInVehicle || !iPlayer.CanInteract()) return false;

            int amount = item.Amount;

            Chats.sendProgressBar(iPlayer, 3000);

            // Remove
            iPlayer.Container.RemoveAllFromSlot(slot);

            iPlayer.Player.TriggerEvent("freezePlayer", true);
            
            iPlayer.PlayAnimation((int)(AnimationFlags.Loop | AnimationFlags.AllowPlayerControl), "amb@prop_human_parking_meter@male@base", "base");

            iPlayer.SetData("userCannotInterrupt", true);

            await Task.Delay(3000);

            iPlayer.SetData("userCannotInterrupt", false);
            iPlayer.StopAnimation();
           
            iPlayer.Player.TriggerEvent("freezePlayer", false);

            iPlayer.GiveBlackMoney(amount);
            iPlayer.SendNewNotification($"Sie haben ${amount} Schwarzgeld entpackt!");
            return true;
        }
    }
}
