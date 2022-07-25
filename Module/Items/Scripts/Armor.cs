using System.Threading.Tasks;
using GTANetworkAPI;
using Nexus.Module.Chat;
using Nexus.Module.Events.Halloween;
using Nexus.Module.Gangwar;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.PlayerAnimations;

namespace Nexus.Module.Items.Scripts
{
    public static partial class ItemScript
    {
        public static async Task<bool> UnderArmor(DbPlayer iPlayer, ItemModel ItemData)
        {
            if (iPlayer.Player.IsInVehicle) return false;
            iPlayer.SetCannotInteract(true);

            Chats.sendProgressBar(iPlayer, 4000);
            iPlayer.PlayAnimation(
                    (int)(AnimationFlags.Loop | AnimationFlags.AllowPlayerControl), Main.AnimationList["fixing"].Split()[0], Main.AnimationList["fixing"].Split()[1]);
            iPlayer.Player.TriggerEvent("freezePlayer", true);

            iPlayer.SetData("armorusing", true);

            await Task.Delay(4000);

            iPlayer.ResetData("armorusing");
            iPlayer.Player.TriggerEvent("freezePlayer", false);
            iPlayer.SetCannotInteract(false);
            iPlayer.StopAnimation();

            iPlayer.SaveArmorType(1);
            iPlayer.VisibleArmorType = 1;
            iPlayer.SetArmor(99, true);

            return true;
        }

        public static async Task<bool> Armor(DbPlayer iPlayer, ItemModel ItemData)
        {
            if (iPlayer.Player.IsInVehicle) return false;
            iPlayer.SetCannotInteract(true);

            Chats.sendProgressBar(iPlayer, 4000);
            iPlayer.PlayAnimation(
                    (int)(AnimationFlags.Loop | AnimationFlags.AllowPlayerControl), Main.AnimationList["fixing"].Split()[0], Main.AnimationList["fixing"].Split()[1]);
            iPlayer.Player.TriggerEvent("freezePlayer", true);

            iPlayer.SetData("armorusing", true);

            await Task.Delay(4000);

            iPlayer.ResetData("armorusing");
            iPlayer.Player.TriggerEvent("freezePlayer", false);
            iPlayer.SetCannotInteract(false);
            iPlayer.StopAnimation();

            iPlayer.SaveArmorType(1);
            iPlayer.VisibleArmorType = 1;
            iPlayer.SetArmor(99, true);

            return true;
        }

        public static async Task<bool> BArmor(DbPlayer iPlayer, ItemModel ItemData)
        {
            if (iPlayer.Player.IsInVehicle || !iPlayer.IsCopPackGun() || !iPlayer.IsInDuty()) return false;
            iPlayer.SetCannotInteract(true);

            Chats.sendProgressBar(iPlayer, 4000);
            iPlayer.PlayAnimation(
                (int)(AnimationFlags.Loop | AnimationFlags.AllowPlayerControl), Main.AnimationList["fixing"].Split()[0], Main.AnimationList["fixing"].Split()[1]);
            iPlayer.Player.TriggerEvent("freezePlayer", true);

            iPlayer.SetData("armorusing", true);

            await Task.Delay(4000);

            iPlayer.ResetData("armorusing");
            iPlayer.Player.TriggerEvent("freezePlayer", false);
            iPlayer.SetCannotInteract(false);
            iPlayer.StopAnimation();
            int type = 30;
            if (iPlayer.VisibleArmorType != type)
                iPlayer.SaveArmorType(type);
            iPlayer.SetArmor(99, true);

            return true;
        }

        public static async Task<bool> BUnderArmor(DbPlayer iPlayer, ItemModel ItemData)
        {
            if (iPlayer.Player.IsInVehicle || !iPlayer.IsCopPackGun() || !iPlayer.IsInDuty()) return false;
            iPlayer.SetCannotInteract(true);

            Chats.sendProgressBar(iPlayer, 4000);
            iPlayer.PlayAnimation(
                    (int)(AnimationFlags.Loop | AnimationFlags.AllowPlayerControl), Main.AnimationList["fixing"].Split()[0], Main.AnimationList["fixing"].Split()[1]);
            iPlayer.Player.TriggerEvent("freezePlayer", true);

            iPlayer.SetData("armorusing", true);

            await Task.Delay(4000);

            iPlayer.ResetData("armorusing");
            iPlayer.Player.TriggerEvent("freezePlayer", false);
            iPlayer.SetCannotInteract(false);
            iPlayer.StopAnimation();

            int type = 30;
            if (iPlayer.VisibleArmorType != type)
                iPlayer.SaveArmorType(type);
            iPlayer.SetArmor(99, true);

            return true;
        }

        public static async Task<bool> FArmor(DbPlayer iPlayer, ItemModel ItemData)
        {
            if (iPlayer.Player.IsInVehicle) return false;
            //if (!iPlayer.Team.IsInTeamfight()) return false;
            if (!GangwarTownModule.Instance.IsTeamInGangwar(iPlayer.Team)) return false;
            iPlayer.SetCannotInteract(true);

            Chats.sendProgressBar(iPlayer, 4000);
            iPlayer.PlayAnimation(
                (int)(AnimationFlags.Loop | AnimationFlags.AllowPlayerControl), Main.AnimationList["fixing"].Split()[0], Main.AnimationList["fixing"].Split()[1]);
            iPlayer.Player.TriggerEvent("freezePlayer", true);

            iPlayer.SetData("armorusing", true);

            await Task.Delay(4000);

            iPlayer.ResetData("armorusing");
            iPlayer.Player.TriggerEvent("freezePlayer", false);
            iPlayer.SetCannotInteract(false);
            iPlayer.StopAnimation();

            iPlayer.SaveArmorType(1);
            iPlayer.VisibleArmorType = 1;
            iPlayer.SetArmor(99, true);

            return true;
        }
    }
}