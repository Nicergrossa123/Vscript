﻿using System;
using System.Collections.Generic;
using System.Linq;
using Nexus.Handler;
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

namespace Nexus.Module.VehicleRent
{
    public class VehicleRentMenuBuilder : MenuBuilder
    {
        public VehicleRentMenuBuilder() : base(PlayerMenu.VehicleRentMenu)
        {
        }

        public override Menu.Menu Build(DbPlayer iPlayer)
        {
            var menu = new Menu.Menu(Menu, "Fahrzeug vermieten");

            menu.Add($"Schließen");

            foreach(SxVehicle SxVeh in VehicleHandler.Instance.GetPlayerVehicles(iPlayer.Id))
            {
                menu.Add($"{SxVeh.databaseId} {(SxVeh.Data.mod_car_name == "" ? SxVeh.Data.Model : SxVeh.Data.mod_car_name)}");
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
                if(index == 0)
                {
                    MenuManager.DismissCurrent(iPlayer);
                    return false;
                }
                else
                {
                    int idx = 1;

                    foreach (SxVehicle SxVeh in VehicleHandler.Instance.GetPlayerVehicles(iPlayer.Id))
                    {
                        if(idx == index)
                        {
                            MenuManager.DismissCurrent(iPlayer);
                            iPlayer.SetData("vehicleRentId", SxVeh.databaseId);

                            // Open User Input
                            ComponentManager.Get<TextInputBoxWindow>().Show()(iPlayer, new TextInputBoxWindowObject() { Title = "Fahrzeug vermieten", Callback = "PlayerRentDays", Message = "Wie lange soll das Fahrzeug vermietet werden?" });
                            return false;
                        }
                        idx++;
                    }
                }
                return false;
            }
        }
    }
}