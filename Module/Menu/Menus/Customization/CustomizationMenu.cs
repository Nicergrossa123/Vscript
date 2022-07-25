using System;
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
    public class CustomizationMenuBuilder : MenuBuilder
    {
        public CustomizationMenuBuilder() : base(PlayerMenu.CustomizationMenu)
        {
        }

        public override Menu Build(DbPlayer iPlayer)
        {
            var menu = new Menu(Menu, "Schönheitsklinik");

            menu.Add($"Schönheitschirugie");
            menu.Add($"Tattoos lasern");

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
                switch (index)
                {
                    case 0:
                        iPlayer.StartCustomization();
                        MenuManager.DismissCurrent(iPlayer);
                        return true;
                    case 1:
                        MenuManager.Instance.Build(PlayerMenu.TattooLaseringMenu, iPlayer).Show(iPlayer);
                        break;
                    default:
                        MenuManager.DismissCurrent(iPlayer);
                        break;
                }
                return false;
            }
        }
    }
}