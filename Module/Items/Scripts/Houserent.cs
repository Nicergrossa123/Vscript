using Nexus.Module.Players;
using Nexus.Module.Players.Db;

namespace Nexus.Module.Items.Scripts
{
    public static partial class ItemScript
    {
        public static bool Houserent(DbPlayer iPlayer, ItemModel ItemData)
        {
            if(iPlayer.ownHouse[0] > 0)
            {
                if (iPlayer.HasData("houseId") && iPlayer.GetData("houseId") == iPlayer.ownHouse[0])
                {
                    Menu.MenuManager.Instance.Build(Menu.PlayerMenu.HouseRentContract, iPlayer).Show(iPlayer);
                    iPlayer.SendNewNotification("Sie stellen nun den Mietvertrag aus!");
                }
                else
                {
                    iPlayer.SendNewNotification("Sie müssen an Ihrem Haus sein!");
                }
            }

            return false;
        }
    }
}