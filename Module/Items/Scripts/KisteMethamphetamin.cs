using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nexus.Handler;
using Nexus.Module.Chat;
using Nexus.Module.Doors;
using Nexus.Module.GTAN;
using Nexus.Module.Injury;
using Nexus.Module.Laboratories;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.JumpPoints;
using Nexus.Module.Players.PlayerAnimations;

namespace Nexus.Module.Items.Scripts
{
    public static partial class ItemScript
    {
        public static async Task<bool> KisteMethamphetamin(DbPlayer dbPlayer, ItemModel ItemData)
        {
            int time = 5000;
            Chats.sendProgressBar(dbPlayer, time);
            Item item = null;
            MethlaboratoryModule.EndProductItemIds.ForEach(id =>
            {
                if (item == null)
                {
                    item = dbPlayer.Container.GetItemById((int)id);
                }
            });
            if (item == null)
            {
                dbPlayer.SendNewNotification("Nichts zum Analysieren gefunden.");
                return false;
            }

            dbPlayer.PlayAnimation((int)(AnimationFlags.Loop | AnimationFlags.AllowPlayerControl), "missheistdockssetup1ig_3@talk", "oh_hey_vin_dockworker");
            dbPlayer.Player.TriggerEvent("freezePlayer", true);
            dbPlayer.SetData("userCannotInterrupt", true);

            await Task.Delay(time);
            dbPlayer.ResetData("userCannotInterrupt");
            if (dbPlayer.IsCuffed || dbPlayer.IsTied || dbPlayer.isInjured()) return false;

            dbPlayer.Player.TriggerEvent("freezePlayer", false);

            if (item == null)
            {
                dbPlayer.SendNewNotification("Nichts zum Wiegen gefunden.");
                return false;
            }
            foreach (KeyValuePair<string, dynamic> keyValuePair in item.Data)
            {
                if (keyValuePair.Key == "amount")
                {
                    string value = Convert.ToString(keyValuePair.Value);
                    dbPlayer.SendNewNotification($"In dieser Kiste befinden sich {value} Kristalle");
                }
            }
            dbPlayer.StopAnimation();
            return true;
        }
    }
}
