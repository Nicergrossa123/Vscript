﻿using GTANetworkAPI;
using Nexus.Module.PlayerUI.Windows;
using Nexus.Module.Items;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Shops.Windows;
using Nexus.Module.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using Nexus.Module.PlayerUI.Components;

namespace Nexus.Module.Shops
{
    public sealed class ShopsModule : Module<ShopsModule>
    {
        public override Type[] RequiredModules()
        {
            return new[] { typeof(TeamModule) };
        }

        public Shop GetThisShop(Vector3 pos, float range = 2.5f)
        {
            return (from kvp in ShopModule.Instance.GetAll() where kvp.Value.Position.DistanceTo(pos) <= range select kvp.Value)
                .FirstOrDefault();
        }

        public Shop GetRobableShopAtPos(Vector3 pos, float range = 2.5f)
        {
            return (from kvp in ShopModule.Instance.GetAll() where kvp.Value.RobPosition.X != 0 && kvp.Value.RobPosition.DistanceTo(pos) <= range select kvp.Value)
                .FirstOrDefault();
        }

        public Shop GetDeliveryShop(Vector3 pos, float range = 2.5f)
        {
            return (from kvp in ShopModule.Instance.GetAll() where kvp.Value.DeliveryPosition.DistanceTo(pos) <= range select kvp.Value)
                .FirstOrDefault();
        }

        public void ResetAllRobStatus()
        {
            foreach (var kvp in ShopModule.Instance.GetAll())
            {
                kvp.Value.Robbed = false;
            }
        }

        public Shop GetShop(int id)
        {
            return ShopModule.Instance.GetAll().ContainsKey((uint)id) ? ShopModule.Instance.GetAll()[(uint)id] : null;
        }

        public void SetShopRobbed(int id, bool robbed = true)
        {
            var shop = GetShop(id);
            if (shop == null) return;
            shop.Robbed = robbed;
        }

        public override bool OnKeyPressed(DbPlayer dbPlayer, Key key)
        {
            if (key == Key.E)
            {
                Shop shop = Instance.GetThisShop(dbPlayer.Player.Position);
                if (shop != null)
                {
                    List<ShopItemX> Items = new List<ShopItemX>();
                    foreach (var item in shop.ShopItems)
                    {
                        Items.Add(new ShopItemX(item.ItemId, item.Name, item.Price, ItemModelModule.Instance.Get(item.ItemId).ImagePath)); // TODO
                    }
                    ComponentManager.Get<ShopWindow>().Show()(dbPlayer, shop.Name, (int)shop.Id, Items);
                    return true;
                }
                // try set shop for delivery
                shop = Instance.GetDeliveryShop(dbPlayer.Player.Position);
                if (shop != null)
                {
                    foreach (ShopItem shopItem in shop.ShopItems.Where(si => si.IsStoredItem))
                    {
                        int neededAmount = shopItem.GetRequiredAmount();
                        int neededChestAmount = neededAmount / 5;
                        if (neededChestAmount > 0) // Shop braucht items...
                        {
                            int playerHasItemAmount = dbPlayer.Container.GetItemAmount((uint)shopItem.RequiredChestItemId);
                            if (playerHasItemAmount > 0)
                            {
                                int resultprice = playerHasItemAmount * 5 * shopItem.EKPrice;
                                dbPlayer.GiveMoney(resultprice);
                                dbPlayer.Container.RemoveItem((uint)shopItem.RequiredChestItemId, playerHasItemAmount);

                                dbPlayer.SendNewNotification($"{playerHasItemAmount} {ItemModelModule.Instance.Get((uint)shopItem.RequiredChestItemId).Name} für ${resultprice} verkauft!");

                                shopItem.Stored += playerHasItemAmount * 5;
                                shopItem.SaveStoreds();
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}