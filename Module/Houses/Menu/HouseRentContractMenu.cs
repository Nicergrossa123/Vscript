using System.Collections.Generic;
using System.Linq;
using Nexus.Handler;
using Nexus.Module.PlayerUI.Components;
using Nexus.Module.Farming;
using Nexus.Module.Menu;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.Windows;
using Nexus.Module.Vehicles;

namespace Nexus.Module.Houses
{
    public class HouseRentContractMenuBuilder : MenuBuilder
    {
        public HouseRentContractMenuBuilder() : base(PlayerMenu.HouseRentContract)
        {
        }

        public override Module.Menu.Menu Build(DbPlayer iPlayer)
        {
            var menu = new Module.Menu.Menu(Menu, "Mietslot wählen");

            menu.Add($"Schließen");
            foreach (HouseRent houseRent in HouseRentModule.Instance.houseRents.ToList().Where(hr => hr.HouseId == iPlayer.ownHouse[0] && hr.PlayerId == 0))
            {
                menu.Add($"Freier Slot {houseRent.SlotId} | ${houseRent.RentPrice}");
            }

            return menu;
        }

        public override IMenuEventHandler GetEventHandler()
        {
            return new EventHandler();
        }

        private class EventHandler : IMenuEventHandler
        {
            public bool OnSelect(int index, DbPlayer iPlayer)
            {
                if (index == 0)
                {
                    MenuManager.DismissCurrent(iPlayer);
                    return false;
                }
                
                
                int count = 1;
                foreach (HouseRent houseRent in HouseRentModule.Instance.houseRents.ToList().Where(hr => hr.HouseId == iPlayer.ownHouse[0] && hr.PlayerId == 0))
                {
                    if (index == count)
                    {
                        iPlayer.SetData("TenantSlot", houseRent.SlotId);
                        ComponentManager.Get<TextInputBoxWindow>().Show()(iPlayer, new TextInputBoxWindowObject() { Title = "Mietvertrag erstellen", Callback = "HouseRentAskTenant", Message = "Hiermit schließen Sie einen Mietvertrag auf dem Mietplatz " + houseRent.SlotId + ". Geben Sie den Namen des Mieters ein:" });

                        MenuManager.DismissCurrent(iPlayer);
                        return true;
                    }
                    else count++;
                }

                MenuManager.DismissCurrent(iPlayer);
                return true;
            }
        }
    }
}