using System;
using System.Collections.Generic;
using System.Linq;
using Nexus.Module.Assets.Tattoo;
using Nexus.Module.Business;
using Nexus.Module.GTAN;
using Nexus.Module.Houses;
using Nexus.Module.Items;
using Nexus.Module.Menu;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Tattoo;

namespace Nexus
{
    public class TattooBuyMenuBuilder : MenuBuilder
    {
        public TattooBuyMenuBuilder() : base(PlayerMenu.TattooBuyMenu)
        {
        }

        public override Menu Build(DbPlayer iPlayer)
        {
            if (!iPlayer.TryData("tattooShopId", out uint tattooShopId)) return null;
            var tattooShop = TattooShopModule.Instance.Get(tattooShopId);
            if (tattooShop == null || tattooShop.BusinessId != 0) return null;

            if(!iPlayer.GetActiveBusinessMember().Owner)
            {
                iPlayer.SendNewNotification("Sie muessen ein Business besitzen!");
            }

            var menu = new Menu(Menu, "TattooShop");

            menu.Add($"Schließen");
            menu.Add($"Shop erwerben {tattooShop.Price}$");

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
                if(index == 0)
                {
                    MenuManager.DismissCurrent(iPlayer);
                    return false;
                }
                if(index == 1)
                {
                    // Buy
                    if (!iPlayer.TryData("tattooShopId", out uint tattooShopId)) return false;
                    var tattooShop = TattooShopModule.Instance.Get(tattooShopId);
                    if (tattooShop == null || tattooShop.BusinessId != 0) return false;

                    if (!iPlayer.GetActiveBusinessMember().Owner)
                    {
                        iPlayer.SendNewNotification("Sie muessen ein Business besitzen!");
                    }

                    if (!iPlayer.TakeMoney(tattooShop.Price))
                    {
                        iPlayer.SendNewNotification(MSG.Money.NotEnoughMoney(tattooShop.Price));
                        return false;
                    }

                    tattooShop.SetBusiness((int)iPlayer.GetActiveBusinessMember().BusinessId);
                    iPlayer.SendNewNotification("Tattoshop erworben!");
                    return true;
                }
                MenuManager.DismissCurrent(iPlayer);
                return false;
            }
        }
    }
}