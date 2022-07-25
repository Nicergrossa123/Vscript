using System;
using System.Globalization;
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
        public static bool Geldboerse(DbPlayer iPlayer, ItemModel ItemData)
        {
            int price = new Random().Next(100, 1100);
            iPlayer.GiveMoney(price);
            iPlayer.SendNewNotification("Geldboerse geoeffnet! $" + price);
            return true;
        }
    }
}