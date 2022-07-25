﻿using GTANetworkAPI;
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
    public class CarsellBuycarSubMenuBuilder : MenuBuilder
    {
        public CarsellBuycarSubMenuBuilder() : base(PlayerMenu.CarsellBuySubMenu)
        {

        }

        public override Module.Menu.Menu Build(DbPlayer p_DbPlayer)
        {
            if (!p_DbPlayer.HasData("carsellCat")) return null;

            var l_Menu = new Module.Menu.Menu(Menu, "Fahrzeug bestellen");
            l_Menu.Add($"Schließen");

            foreach (VehicleData vehData in VehicleDataModule.Instance.data.Values.ToList().Where(vd => vd.IsShopVehicle && vd.CarsellCategory.Id == p_DbPlayer.GetData("carsellCat")))
            {
                l_Menu.Add($"{(vehData.mod_car_name.Length <=0 ? vehData.Model : vehData.mod_car_name)}");
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
                foreach (VehicleData vehData in VehicleDataModule.Instance.data.Values.ToList().Where(vd => vd.IsShopVehicle && vd.CarsellCategory.Id == iPlayer.GetData("carsellCat")))
                {
                    if(idx == index)
                    {
                        // Fahrzeug zum Bestellen

                        if(!JobCarsellFactionModule.Instance.CanAddVehicle(iPlayer.TeamId))
                        {
                            iPlayer.SendNewNotification("Sie haben bereits die maximale Anzahl an Vorführfahrzeugen erreicht!");
                            return true;
                        }

                        if (vehData.CarsellCategory.Limit != 0)
                        {
                            if (JobCarsellFactionModule.Instance.GetCategoryAmount(vehData.CarsellCategory.Id, iPlayer.TeamId) >= vehData.CarsellCategory.Limit)
                            {
                                iPlayer.SendNewNotification($"Sie können nur {vehData.CarsellCategory.Limit} dieses Fahrzeugtypes besitzen!");
                                return true;
                            }
                        }


                        TeamShelter teamShelter = TeamShelterModule.Instance.GetByTeam(iPlayer.TeamId);
                        if (teamShelter == null) return false;
                        if (teamShelter.Money < JobCarsellFactionModule.FVehicleBuyPrice)
                        {
                            iPlayer.SendNewNotification("Eine Bestellung kostet 10.000$ (Fraktionsbank)!");
                            return true;
                        }

                        // Remove Money
                        teamShelter.TakeMoney(JobCarsellFactionModule.FVehicleBuyPrice);

                        // Insert Into Orders
                        MySQLHandler.ExecuteAsync($"INSERT INTO `jobfaction_carsell_fvorder` (`team_id`, `player_id`, `vehicle_data_id`) VALUES ('{iPlayer.TeamId}', '{iPlayer.Id}', '{vehData.Id}');");
                        iPlayer.SendNewNotification($"Fahrzeug {(vehData.mod_car_name.Length <= 0 ? vehData.Model : vehData.mod_car_name)} wurde bestellt! (Nächste Sonnenwende)");
                        return true;
                    }
                    idx++;
                }

                MenuManager.DismissCurrent(iPlayer);
                return true;
            }
        }
    }
}
