using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nexus.Handler;
using Nexus.Module.PlayerUI.Components;
using Nexus.Module.JobFactions.Carsell;
using Nexus.Module.Menu;
using Nexus.Module.NSA.Observation;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.Windows;
using Nexus.Module.Teams.Shelter;
using Nexus.Module.Telefon.App;
using Nexus.Module.Vehicles.Data;

namespace Nexus.Module.Carsell.Menu
{
    public class CarsellBuycarMenuBuilder : MenuBuilder
    {
        public CarsellBuycarMenuBuilder() : base(PlayerMenu.CarsellBuyMenu)
        {

        }

        public override Module.Menu.Menu Build(DbPlayer p_DbPlayer)
        {
            var l_Menu = new Module.Menu.Menu(Menu, "Fahrzeug bestellen");
            l_Menu.Add($"Schließen");

            foreach(VehicleCarsellCategory vehicleCarsellCategory in VehicleCarsellCategoryModule.Instance.GetAll().Values)
            {
                l_Menu.Add($"{vehicleCarsellCategory.Name}");
            }
            
            return l_Menu;
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
                    return true;
                }

                int idx = 1;

                foreach (VehicleCarsellCategory vehicleCarsellCategory in VehicleCarsellCategoryModule.Instance.GetAll().Values)
                {
                    if(idx == index)
                    {
                        iPlayer.SetData("carsellCat", vehicleCarsellCategory.Id);
                        Module.Menu.MenuManager.Instance.Build(Nexus.Module.Menu.PlayerMenu.CarsellBuySubMenu, iPlayer).Show(iPlayer);
                        return false;
                    }
                    idx++;
                }

                MenuManager.DismissCurrent(iPlayer);
                return true;
            }
        }
    }
}
