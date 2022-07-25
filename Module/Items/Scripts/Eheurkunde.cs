using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nexus.Module.Chat;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using static Nexus.Module.Chat.Chats;

namespace Nexus.Module.Items.Scripts
{
    public static partial class ItemScript
    {
        public static bool Eheurkunde(DbPlayer iPlayer, Item item)
        {
            if (item.Data == null) return false;
            if (!item.Data.ContainsKey("Info")) return false;
            string info = (string) item.Data["Info"];

            iPlayer.SendNewNotification(info, PlayerNotification.NotificationType.INFO, "Eheurkunde", 15000);

            return false;
        }

        public static async Task<bool> MarryAnnounce(DbPlayer iPlayer, Item item)
        {
            if (item.Data == null) return false;
            if (!item.Data.ContainsKey("Info")) return false;
            string info = (string)item.Data["Info"];

            await Chats.SendGlobalMessage(info, COLOR.WHITE, ICON.WED, duration: 20000);

            foreach (var pPlayer in Players.Players.Instance.GetValidPlayers())
            {
                pPlayer.SendNewNotification("1337Allahuakbar$marry", duration: 20000);
            }

            return true;
        }
    }
}
