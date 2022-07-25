using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nexus.Module.PlayerUI.Components;
using Nexus.Module.Clothes.Props;
using Nexus.Module.Menu;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.Windows;

namespace Nexus.Module.Clothes.Outfits
{
    public class OutfitsMenuBuilder : MenuBuilder
    {
        public OutfitsMenuBuilder() : base(PlayerMenu.OutfitsMenu)
        {
        }

        public override Menu.Menu Build(DbPlayer iPlayer)
        {
            var menu = new Menu.Menu(Menu, "Outfits");
            menu.Add(MSG.General.Close());
            menu.Add("Aktuelles Outfit speichern");
            foreach (Outfit outfit in iPlayer.Outfits.ToList())
            {
                menu.Add(outfit.Name, "");
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
                    MenuManager.DismissMenu(iPlayer.Player, (uint)PlayerMenu.OutfitsMenu);
                    ClothModule.SaveCharacter(iPlayer);
                    return false;
                }
                else if (index == 1)
                {
                    // Saving...
                    ComponentManager.Get<TextInputBoxWindow>().Show()(iPlayer, new TextInputBoxWindowObject() { Title = "Outfit speichern", Callback = "SaveOutfit", Message = "Wie soll das Outfit heißen?" });
                    return true;
                }

                int idx = 2;
                foreach (Outfit outfit in iPlayer.Outfits.ToList())
                {
                    if(idx == index)
                    {
                        iPlayer.SetData("outfit", outfit);
                        MenuManager.Instance.Build(PlayerMenu.OutfitsSubMenu, iPlayer).Show(iPlayer);
                        return false;
                    }
                    idx++;
                }
                return false;
            }
        }
    }
}