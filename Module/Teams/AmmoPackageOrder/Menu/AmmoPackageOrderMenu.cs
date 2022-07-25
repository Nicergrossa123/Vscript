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
using Nexus.Module.Teams.Shelter;

namespace Nexus.Module.Teams.AmmoPackageOrder
{
    public class AmmoPackageOrderMenuBuilder : MenuBuilder
    {
        public AmmoPackageOrderMenuBuilder() : base(PlayerMenu.AmmoPackageOrderMenu)
        {
        }

        public override Menu.Menu Build(DbPlayer iPlayer)
        {
            if (iPlayer.Team.Id != (int)teams.TEAM_HUSTLER && iPlayer.Team.Id != (int)teams.TEAM_ICA) return null;
            if (iPlayer.TeamRank < 8) return null;

            var menu = new Menu.Menu(Menu, "Munitionsbestellung");

            menu.Add($"Schließen");
            foreach(DbTeam dbTeam in TeamModule.Instance.GetAll().Values.Where(t => t.IsGangsters()))
            {
                menu.Add("Bestellung " + dbTeam.Name);
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
                else // choose x.x
                {
                    int idx = 1;
                    foreach (DbTeam dbTeam in TeamModule.Instance.GetAll().Values.Where(t => t.IsGangsters()))
                    {
                        if (idx == index)
                        {
                            iPlayer.SetData("orderedTeam", dbTeam.Id);
                            ComponentManager.Get<TextInputBoxWindow>().Show()(iPlayer, new TextInputBoxWindowObject() { Title = "Kisten Anzahl", Callback = "AddAmmoPackageOrder", Message = "Geben Sie die Anzahl an Kisten an." });
                            return true;
                        }
                        else idx++;
                    }
                    return true;
                }
            }
        }
    }
}