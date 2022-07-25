using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GTANetworkAPI;
using Nexus.Handler;
using Nexus.Module.Camper;
using Nexus.Module.Chat;
using Nexus.Module.Menu;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.PlayerAnimations;
using Nexus.Module.Vehicles;

namespace Nexus.Module.Items.Scripts
{
    public static partial class ItemScript
    {
        public static async Task<bool> CCook(DbPlayer iPlayer, ItemModel ItemData)
        {

            if (iPlayer.Player.IsInVehicle) return false;
            CampingPlace campingPlace = CampingModule.Instance.CampingPlaces.ToList().Where(cp => cp.Position.DistanceTo(iPlayer.Player.Position) < 7.0f).FirstOrDefault();
            if (campingPlace != null)
            {
                if (campingPlace.GrillPosition == new Vector3(0, 0, 0)) return false;

                if (campingPlace.GrillPosition.DistanceTo(iPlayer.Player.Position) < 2.0f)
                {
                    string[] args = ItemData.Script.Split('_');
                    if (!UInt32.TryParse(args[1], out uint newItemId)) return false;
                    ItemModel newItem = ItemModelModule.Instance.Get(newItemId);
                    if (newItem == null) return false;

                    if(!iPlayer.Container.CanInventoryItemAdded(newItem, 1))
                    {
                        iPlayer.SendNewNotification("Du hast nicht genug platz im Inventar!");
                        return false;
                    }

                    Attachments.AttachmentModule.Instance.AddAttachment(iPlayer, 64, true);

                    iPlayer.PlayAnimation(
                        (int)(AnimationFlags.Loop | AnimationFlags.AllowPlayerControl), "amb@world_human_drinking@coffee@female@base", "base");
                    iPlayer.Player.TriggerEvent("freezePlayer", true);
                    iPlayer.SetData("userCannotInterrupt", true);

                    Chats.sendProgressBar(iPlayer, 20000);
                    await Task.Delay(20000);

                    iPlayer.ResetData("userCannotInterrupt");
                    iPlayer.Player.TriggerEvent("freezePlayer", false);
                    iPlayer.StopAnimation();

                    campingPlace.SetLastUsedNow();

                    iPlayer.SendNewNotification($"Du hast {ItemData.Name} gegrillt!");

                    iPlayer.Container.AddItem(newItemId, 1);
                    return true;
                }
            }

            return false;
        }
    }
}