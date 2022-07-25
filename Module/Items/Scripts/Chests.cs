using System.Threading.Tasks;
using GTANetworkAPI;
using Nexus.Module.Blitzer;
using Nexus.Module.Chat;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.PlayerAnimations;

namespace Nexus.Module.Items.Scripts
{
    public static partial class ItemScript
    {
        public static bool ChestUnpack(DbPlayer iPlayer, ItemModel ItemData)
        {
            string itemScript = ItemData.Script;

            if(!uint.TryParse(itemScript.Split('_')[1], out uint itemModelId))
            {
                return false;
            }

            if(!int.TryParse(itemScript.Split('_')[2], out int itemAmount))
            {
                return false;
            }

            if (itemModelId == 40) // Schutzweste
            {
                switch (iPlayer.TeamId)
                {
                    case (int)teams.TEAM_FIB:
                        itemModelId = 712;
                        break;
                    case (int)teams.TEAM_ARMY:
                        itemModelId = 722;
                        break;
                    case (int)teams.TEAM_POLICE:
                        itemModelId = 697;
                        break;
                    default:
                        break;
                }
            }

            ItemModel itemModel = ItemModelModule.Instance.Get(itemModelId);
            int addedWeight = itemModel.Weight * itemAmount;

            if((iPlayer.Container.GetInventoryFreeSpace() + ItemData.Weight) < addedWeight)
            {
                iPlayer.SendNewNotification("So viel kannst du nicht tragen!");
                return false;
            }

            iPlayer.Container.AddItem(itemModel, itemAmount);
            return true;
        }
    }
}