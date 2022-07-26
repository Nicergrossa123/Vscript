﻿using System.Threading.Tasks;
using GTANetworkAPI;
using Nexus.Module.Chat;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.PlayerAnimations;

namespace Nexus.Module.Items.Scripts
{
    public static partial class ItemScript
    {
        public static async Task<bool> Drink(DbPlayer iPlayer, ItemModel ItemData)
        {
            if (!iPlayer.CanInteract() || iPlayer.Player.IsInVehicle) return false;

            iPlayer.PlayAnimation((int)(AnimationFlags.AllowPlayerControl | AnimationFlags.Loop | AnimationFlags.OnlyAnimateUpperBody), "amb@world_human_drinking@coffee@male@idle_a", "idle_a");
            iPlayer.SetCannotInteract(true);
            await Task.Delay(5000);
            iPlayer.SetCannotInteract(false);
            iPlayer.StopAnimation();
            return true;
        }
        public static async Task<bool> AttachedDrink(DbPlayer iPlayer, ItemModel ItemData)
        {
            if (!iPlayer.CanInteract() || iPlayer.Player.IsInVehicle) return false;


            if (!int.TryParse(ItemData.Script.Split("_")[1], out int type)) return false;

            iPlayer.StopAnimation();

            Module.Attachments.AttachmentModule.Instance.AddAttachment(iPlayer, type);

            iPlayer.StopAnimation(Module.Players.PlayerAnimations.AnimationLevels.User, true);

            await Task.Delay(500);

            iPlayer.PlayAnimation((int)(AnimationFlags.AllowPlayerControl | AnimationFlags.Loop | AnimationFlags.OnlyAnimateUpperBody), "amb@world_human_drinking@coffee@male@idle_a", "idle_a");
            iPlayer.SetCannotInteract(true);
            await Task.Delay(5000);
            iPlayer.SetCannotInteract(false);
            iPlayer.StopAnimation();
            return true;
        }
    }
}