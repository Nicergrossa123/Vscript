using System;
using System.Collections.Generic;
using System.Linq;
using Nexus.Module.Assets.Tattoo;
using Nexus.Module.Business;
using Nexus.Module.Houses;
using Nexus.Module.Items;
using Nexus.Module.Menu;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Tattoo;

namespace Nexus
{
    public class TattooLicensePaginationMenuBuilder : MenuBuilder
    {
        public TattooLicensePaginationMenuBuilder() : base(PlayerMenu.TattooLicensePaginationMenu)
        {
        }

        public override Menu Build(DbPlayer iPlayer)
        {
            if (!iPlayer.HasTattooShop())
            {
                iPlayer.SendNewNotification("Du besitzt keinen Tattoo-Shop und kannst entsprechend keine Lizenzen erwerben!", PlayerNotification.NotificationType.ERROR);
                return null;
            }

            List<TattooLicense> licenses = TattooLicenseModule.Instance.GetAll().Values.ToList();
            if (licenses == null || licenses.Count == 0)
            {
                iPlayer.SendNewNotification("Es werden aktuell keine Tattoo-Lizenzen zum Kauf angeboten!");
                return null;
            }

            uint PagesAmount = (uint) (licenses.Count() / 100);
            if (PagesAmount == 0)
                PagesAmount = 1;

            var tattooshop = TattooShopFunctions.GetTattooShop(iPlayer);
            if (tattooshop == null)
            {
                iPlayer.SendNewNotification("Der Lizenzenshop konnte dich keinem Tattooladen zuordnen. Melde dies bitte im GVMP-Bugtracker!", PlayerNotification.NotificationType.ERROR);
                return null;
            }

            var menu = new Menu(Menu, "Tattoo Licenses");
            menu.Add(MSG.General.Close());

            for (uint itr = 0; itr < PagesAmount; itr++)
            {
                menu.Add($"Seite {itr + 1}");
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
                if (index > 0)
                {
                    uint page = (uint)index;
                    iPlayer.SetData("tattooLicensePage", page);
                    MenuManager.Instance.Build(PlayerMenu.TattooLicenseMenu, iPlayer).Show(iPlayer);
                    return false;
                }
                else
                {
                    MenuManager.DismissCurrent(iPlayer);
                    return false;
                }
            }
        }
    }
}