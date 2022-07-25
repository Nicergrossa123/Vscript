using System;
using System.Collections.Generic;
using System.Linq;
using Nexus.Module.Assets.Tattoo;
using Nexus.Module.Business;
using Nexus.Module.PlayerUI.Components;
using Nexus.Module.GTAN;
using Nexus.Module.Houses;
using Nexus.Module.Items;
using Nexus.Module.Menu;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.Windows;
using Nexus.Module.Tattoo;

namespace Nexus.Module.Warehouse
{
    public class WarehouseSellMenuBuilder : MenuBuilder
    {
        public WarehouseSellMenuBuilder() : base(PlayerMenu.WarehouseSellMenu)
        {
        }

        public override Menu.Menu Build(DbPlayer iPlayer)
        {
            Warehouse warehouse = WarehouseModule.Instance.GetThis(iPlayer.Player.Position);
            if (warehouse == null) return null;

            var menu = new Menu.Menu(Menu, "Warenlager Verkauf");

            menu.Add($"Schließen");

            foreach (WarehouseItem warehouseItem in warehouse.WarehouseItems)
            {
                menu.Add($"{ItemModelModule.Instance.Get(warehouseItem.ResultItemId).Name} ${warehouseItem.ResultItemPrice}");
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
                Warehouse warehouse = WarehouseModule.Instance.GetThis(iPlayer.Player.Position);
                if (warehouse == null) return false;
                if (index == 0)
                {
                    MenuManager.DismissCurrent(iPlayer);
                    return false;
                }
                else
                {
                    int idx = 1;
                    foreach (WarehouseItem warehouseItem in warehouse.WarehouseItems)
                    {
                        if (idx == index)
                        {
                            // Check is enough there...
                            
                            if (warehouseItem.ResultItemBestand > 0)
                            {
                                if(!iPlayer.Container.CanInventoryItemAdded(warehouseItem.ResultItemId))
                                {
                                    iPlayer.SendNewNotification("Sie haben nicht genug platz im Inventar!");
                                    return false;
                                }

                                if (iPlayer.TakeMoney(warehouseItem.ResultItemPrice))
                                {
                                    // Add Players Items...
                                    iPlayer.Container.AddItem(warehouseItem.ResultItemId);

                                    // Add To bestand..
                                    warehouseItem.ResultItemBestand -= 1;
                                    warehouseItem.UpdateBestand();

                                    iPlayer.SendNewNotification($"Sie haben {ItemModelModule.Instance.Get(warehouseItem.ResultItemId).Name} für ${warehouseItem.ResultItemPrice} verkauft!");
                                }
                                else
                                {
                                    iPlayer.SendNewNotification($"Sie haben nicht genug Geld dabei!");
                                    return false;
                                }
                            }
                            return true;
                        }
                        idx++;
                    }
                    return true;
                }
            }
        }
    }
}