using Nexus.Module.Clothes;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;

namespace Nexus.Module.Items.Scripts
{
    public static partial class ItemScript
    {
        public static bool Divebottle(DbPlayer iPlayer, ItemModel itemModel)
        {
            int texture = int.Parse(itemModel.Script.Split("_")[1]);
            iPlayer.SetClothes(8, 123, texture);

            iPlayer.SendNewNotification("Taucherflasche angezogen");

            return true;
        }
    }
}