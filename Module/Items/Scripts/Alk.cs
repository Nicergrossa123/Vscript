using System;
using System.Threading.Tasks;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.Drunk;

namespace Nexus.Module.Items.Scripts
{
    public static partial class ItemScript
    {
        public static async Task<bool> Alk(DbPlayer iPlayer, ItemModel ItemData)
        {
            if (!iPlayer.CanInteract()) return false;

            iPlayer.PlayAnimation((int)(AnimationFlags.AllowPlayerControl | AnimationFlags.Loop | AnimationFlags.OnlyAnimateUpperBody),
                    "amb@world_human_drinking@coffee@male@idle_a",
                    "idle_a");

            iPlayer.SetCannotInteract(true);
            await Task.Delay(5000);
            iPlayer.SetCannotInteract(false);
            var level = Convert.ToInt32(ItemData.Script.Split("_")[1]);
            DrunkModule.Instance.IncreasePlayerAlkLevel(iPlayer, level);
            iPlayer.StopAnimation();

            return true;
        }
    }
}
