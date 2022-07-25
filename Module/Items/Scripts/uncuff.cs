using System.Collections.Generic;
using System.Linq;
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
        public static async Task<bool> uncuff(DbPlayer iPlayer, ItemModel ItemData)
        {
            if (!iPlayer.CanInteract())
            {
                return false;
            }
            
            foreach (DbPlayer xPlayer in Players.Players.Instance.GetValidPlayers().Where(xp => xp.Player.Position.DistanceTo(iPlayer.Player.Position) < 3.0f))
            {
                if ((iPlayer.Player != xPlayer.Player) && (xPlayer.IsTied || xPlayer.IsCuffed || xPlayer.HasData("follow")))
                {
                    // Wenn Spieler in Range, gecufft oder gefesselt ist
                    iPlayer.SendNewNotification(
                         "Sie versuchen die Fesseln zu knacken...");

                    Chats.sendProgressBar(iPlayer, 5000);

                    
                        iPlayer.PlayAnimation((int)(AnimationFlags.Loop | AnimationFlags.AllowPlayerControl), "mp_arresting", "a_uncuff");
                        iPlayer.Player.TriggerEvent("freezePlayer", true);

                        await Task.Delay(5000);

                        // Recheck Distance
                        if (xPlayer.Player.Position.DistanceTo(iPlayer.Player.Position) > 3.0f) return false;

                        iPlayer.Player.TriggerEvent("freezePlayer", false);
                        iPlayer.StopAnimation();
                    xPlayer.SetCuffed(false);

                        xPlayer.SendNewNotification(
                             iPlayer.GetName() +
                            " hat Ihre Handschellen gelöst!");
                        iPlayer.SendNewNotification(
                            
                            "Sie haben die Handschellen von " +
                            xPlayer.GetName() + " gelöst!");
                        return true;
                    
                    return true;
                }
            }
            return false;
        }
    }
}