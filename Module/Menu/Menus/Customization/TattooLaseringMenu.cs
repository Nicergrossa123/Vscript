﻿using System;
using System.Collections.Generic;
using System.Linq;
using Nexus.Module.Assets.Tattoo;
using Nexus.Module.Customization;
using Nexus.Module.Houses;
using Nexus.Module.Items;
using Nexus.Module.Menu;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Tattoo;

namespace Nexus
{
    public class TattooLaseringMenuBuilder : MenuBuilder
    {
        public TattooLaseringMenuBuilder() : base(PlayerMenu.TattooLaseringMenu)
        {
        }

        public override Menu Build(DbPlayer iPlayer)
        {
            var menu = new Menu(Menu, "Tattoo Lasering");
            
            foreach(uint id in iPlayer.Customization.Tattoos)
            {
                int price = 200 * iPlayer.Level;

                AssetsTattoo assetsTattoo = AssetsTattooModule.Instance.Get(id);
                if (assetsTattoo == null) continue;
                menu.Add($"{assetsTattoo.Name} {price}$");
            }

            menu.Add(MSG.General.Close());
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
                int idx = 0;

                
                foreach (uint id in iPlayer.Customization.Tattoos)
                {
                    if (AssetsTattooModule.Instance.GetAll().ContainsKey(id))
                    {
                        if(idx == index)
                        {
                            int price = 200 * iPlayer.Level;

                            if(!iPlayer.TakeMoney(price))
                            {
                                iPlayer.SendNewNotification(MSG.Money.NotEnoughMoney(price));
                                return false;
                            }

                            iPlayer.SendNewNotification($"Tattoo {AssetsTattooModule.Instance.Get(id).Name} entfernt, kosten: {price}$");
                            iPlayer.LaserTattoo(id);
                            return true;
                        }
                        idx++;
                    }
                }

                MenuManager.DismissCurrent(iPlayer);
                return false;
            }
        }
    }
}