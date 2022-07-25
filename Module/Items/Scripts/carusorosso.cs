using System.Threading.Tasks;
using GTANetworkAPI;
using Nexus.Module.Chat;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.PlayerAnimations;

namespace Nexus.Module.Items.Scripts
{
    public static partial class ItemScript
    {
        public static async Task<bool> carusorosso(DbPlayer iPlayer, ItemModel ItemData)
        {
            
                iPlayer.PlayAnimation( 
                    (int) (AnimationFlags.Loop | AnimationFlags.AllowPlayerControl), Main.AnimationList["bro"].Split()[0], Main.AnimationList["bro"].Split()[1]); 
                iPlayer.Player.TriggerEvent("freezePlayer", true); 
                await Task.Delay(3000); 
                iPlayer.Player.TriggerEvent("freezePlayer", false); 
                iPlayer.StopAnimation(); 
            
            return true;
        }
    }
}