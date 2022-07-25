﻿using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Nexus.Handler;
using Nexus.Module.Chat;
using Nexus.Module.PlayerUI.Components;
using Nexus.Module.GTAN;
using Nexus.Module.Items;
using Nexus.Module.Logging;
using Nexus.Module.Menu;
using Nexus.Module.NSA;
using Nexus.Module.Players;
using Nexus.Module.Players.Windows;
using Nexus.Module.RemoteEvents;
using Nexus.Module.Staatskasse;
using Nexus.Module.Teams;
using Nexus.Module.Vehicles;

namespace Nexus.Module.Business.FuelStations
{
    class RaffineryEvents : Script
    {
        [RemoteEvent]
        public void SetFuelName(Player player, string returnstring)
        {
            var dbPlayer = player.GetPlayer();
            if (dbPlayer == null) return;
            
            if (!dbPlayer.TryData("fuelstationId", out uint fuelStationId)) return;
            var fuelstation = FuelStationModule.Instance.Get(fuelStationId);
            if (fuelstation == null) return;

            if (fuelstation.IsOwnedByBusines())
            {
                if (fuelstation.GetOwnedBusiness() == dbPlayer.ActiveBusiness && dbPlayer.GetActiveBusinessMember() != null && dbPlayer.GetActiveBusinessMember().Fuelstation) // Member of business and has rights
                {
                    if(Regex.IsMatch(returnstring, @"^[a-zA-Z ]+$") && FuelStationModule.Instance.GetAll().Where(fs => fs.Value.Name.ToLower() == returnstring.ToLower()).Count() == 0)
                    {
                        fuelstation.SetFuelName(returnstring);
                        dbPlayer.SendNewNotification("Name wurde geaendert!");
                    }
                    else
                    {
                        dbPlayer.SendNewNotification("Dieser Name ist nicht gueltig!");
                        return;
                    }
                }
            }
        }

        [RemoteEvent]
        public void SetFuelPrice(Player player, string returnstring)
        {
            var dbPlayer = player.GetPlayer();
            if (dbPlayer == null) return;

            int price = 0;
            if (!Int32.TryParse(returnstring, out price))
            {
                dbPlayer.SendNewNotification("Dieser Preis kann nicht gesetzt werden!");
                return;
            }
            
            // check realistic value
            if (price < 1 || price > 100)
            {
                dbPlayer.SendNewNotification("Dieser Preis kann nicht gesetzt werden!");
                return;
            }

            if (!dbPlayer.TryData("fuelstationId", out uint fuelStationId)) return;
            var fuelstation = FuelStationModule.Instance.Get(fuelStationId);
            if (fuelstation == null) return;

            if (fuelstation.IsOwnedByBusines())
            {
                if (fuelstation.GetOwnedBusiness() == dbPlayer.ActiveBusiness && dbPlayer.GetActiveBusinessMember() != null && dbPlayer.GetActiveBusinessMember().Fuelstation) // Member of business and has rights
                {
                    fuelstation.SetFuelPrice(price);
                    dbPlayer.SendNewNotification($"Preis wurde auf ${price} gesetzt!");
                }
            }
        }

        [RemoteEvent]
        public void fillfuelstation(Player player, string returnstring)
        {
            var dbPlayer = player.GetPlayer();
            if (dbPlayer == null) return;

            int liter = 0;
            if (!Int32.TryParse(returnstring, out liter) || liter < 1 || liter > 2000)
            {
                dbPlayer.SendNewNotification("Falsche Menge!");
                return;
            }

            dbPlayer.SetData("fillLiter", liter);
            MenuManager.Instance.Build(PlayerMenu.FuelStationFillMenu, dbPlayer).Show(dbPlayer);
            return;
        }

        [RemoteEvent]
        public void fillvehicle(Player player, string returnstring)
        {
            var dbPlayer = player.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid()) return;
            if (!dbPlayer.CanAccessRemoteEvent() || player.IsInVehicle) return;
            var dbVehicle = VehicleHandler.Instance.GetClosestVehicle(dbPlayer.Player.Position, 8.0f);
            if (dbVehicle == null || !dbVehicle.IsValid()) return;
            if (!dbVehicle.CanInteract) return;

            // check for gas stations
            var fuel = FuelStationModule.Instance.GetStaionByGas(dbPlayer.Player.Position);

            if (fuel == null) return;

            int liter = 0;
            if (!Int32.TryParse(returnstring, out liter))
            {
                dbPlayer.SendNewNotification("Falsche Literangabe!");
                return;
            }

            // check realistic value
            if (liter < 1 || liter > dbVehicle.Data.Fuel)
            {
                dbPlayer.SendNewNotification("Falsche Literangabe!");
                return;
            }

            // price
            var fueltoget = Convert.ToInt32(dbVehicle.Data.Fuel - dbVehicle.fuel);

            if (liter > fueltoget)
            {
                dbPlayer.SendNewNotification("Soviel passt in den Tank nicht rein!");
                return;
            }

            var priceperliter = fuel.Price;
            //Todo: toInt32 throws excpetion when overflow
            var price = Convert.ToInt32(priceperliter * liter);
            if (price == 0)
            {
                price = 1;
            }

            if (fueltoget <= 0 || fueltoget < 1)
            {
                dbPlayer.SendNewNotification(

                    "Das Fahrzeug ist voll und kann nicht betankt werden. Mindestmenge 1l");
                return;
            }

            if (fuel.IsOwnedByBusines() && fuel.Container.GetItemAmount(FuelStationModule.BenzinModelId) < liter)
            {
                dbPlayer.SendNewNotification(
                        "Diese Tankstelle hat nicht genug Benzin!", notificationType: PlayerNotification.NotificationType.ERROR);
                return;
            }

            bool dutyFuel = false;

            // not enough money
            if (!dbVehicle.Team.IsCops() && !dbVehicle.Team.IsMedics() && dbVehicle.teamid != (int)teams.TEAM_DPOS && dbVehicle.teamid != (int)teams.TEAM_FIB && dbVehicle.teamid != (int)teams.TEAM_NEWS && dbVehicle.teamid != (int)teams.TEAM_GOV && dbVehicle.teamid != (int)teams.TEAM_DRIVINGSCHOOL)
            {
                if (!dbPlayer.TakeMoney(price))
                {
                    dbPlayer.SendNewNotification(
                            MSG.Money.NotEnoughMoney(price));
                    return;
                }
            }
            else
            {
                //Staatsfraktions auto
                // IAA Special wegen Secret Service
                if ((dbVehicle.Team.Id == dbPlayer.Team.Id) || (dbVehicle.Team.Id == (int)teams.TEAM_GOV && dbPlayer.Team.Id == (int)teams.TEAM_FIB))
                {
                    dbPlayer.SendNewNotification("Rechnung wird vom Arbeitgeber getragen!");
                    //TODO Tankkarte
                    KassenModule.Instance.ChangeMoney(KassenModule.Kasse.STAATSKASSE, -price);
                    dutyFuel = true;
                }
                else
                {
                    dbPlayer.SendNewNotification("Dieses Fahrzeug kannst du nicht tanken");
                    return;
                }
            }

            //Recheck
            if (fuel.IsOwnedByBusines() && fuel.Container.GetItemAmount(FuelStationModule.BenzinModelId) < liter)
            {
                dbPlayer.SendNewNotification(
                        "Diese Tankstelle hat nicht genug Benzin!", notificationType: PlayerNotification.NotificationType.ERROR);
                return;
            }

            // reset fuel fill counter
            dbVehicle.fuel = dbVehicle.fuel + liter;
            dbVehicle.CanInteract = true;

            // send confirmation messages
            dbPlayer.SendNewNotification(
                "Sie haben " + liter + " Liter fuer $" + price + " getankt ($" +
                priceperliter + "/Liter)");
            Logger.AddFuelToDB((float)fueltoget, (int)dbPlayer.Team.Id);
            dbPlayer.Player.TriggerEvent("updateFuel", Convert.ToInt32(dbVehicle.Data.Fuel));
            fuel.Container.RemoveItem(FuelStationModule.BenzinModelId, liter);

            // Log to Fuelstations Log (Need here before subvention)...
            fuel.LogFuelStationProgress(dbPlayer, liter, price, dutyFuel);

            if (fuel.IsOwnedByBusines())
            {
                fuel.GetOwnedBusiness().GiveMoney(price);
            }

            if (dbPlayer.HasMoneyTransferWantedStatus() && !dbPlayer.IsMasked())
            {
                TeamModule.Instance.SendMessageToTeam($"Finanz-Detection: Die Gesuchte Person {dbPlayer.GetName()} einen Tankvorgang getätigt! (Standort: {fuel.Name})", teams.TEAM_FIB, 10000, 3);
                NSAPlayerExtension.AddTransferHistory($"{dbPlayer.GetName()} Tankvorgang {fuel.Name}", fuel.Position);
            }
        }

        [RemoteEventPermission]
        [RemoteEvent]
        public void REQUEST_VEHICLE_FILL_FUEL(Player Player, Vehicle vehicle)
        {
            var dbPlayer = Player.GetPlayer();
            if (dbPlayer == null || !dbPlayer.CanAccessRemoteEvent() || Player.IsInVehicle) return;
            var dbVehicle = vehicle.GetVehicle();
            if (!dbVehicle.IsValid()) return;
            if (dbVehicle.SyncExtension.Locked) return;
            if (dbPlayer.Player.Position.DistanceTo(vehicle.Position) > 20f) return;

            // check for gas stations
            var fuel = FuelStationModule.Instance.GetStaionByGas(dbPlayer.Player.Position);
            
            // no gas station found
            if (fuel == null)
            {
                dbPlayer.SendNewNotification(
                     "Sie muessen an einer Tankstelle sein!");
                return;
            }
            
            // enigne is running
            if (dbVehicle.SyncExtension.EngineOn || dbVehicle.SyncExtension.Locked)
            {
                dbPlayer.SendNewNotification(
                    
                    "Motor muss abgeschaltet und Fahrzeug aufgeschlossen sein!");
                return;
            }
            
            int fuelstationFuelAmount = fuel.Container.GetItemAmount(FuelStationModule.BenzinModelId);
            bool isOwned = fuel.GetOwnedBusiness() == null ? false : true;
            if (fuelstationFuelAmount <= 0 && isOwned)
            {
                dbPlayer.SendNewNotification("Diese Tankstelle hat kein Benzin verfügbar", PlayerNotification.NotificationType.ERROR, "Tankstelle");
                return;
            }

            int vehicleFuelNeeded = dbVehicle.Data.Fuel - (int)Math.Ceiling(dbVehicle.fuel);
            if (vehicleFuelNeeded <= 0)
            {
                dbPlayer.SendNewNotification("Dein Fahrzeug ist bereits vollgetankt", PlayerNotification.NotificationType.ERROR, "Tankstelle");
                return;
            }


            // Autos mit Kerosion - GEILE IDEE
            if (FuelStationModule.AirportFuels.Contains(fuel.Id) && !FuelStationModule.AirVehicleDataGroups.Contains(dbVehicle.Data.ClassificationId)) // allow airplanes, helicopter only on airport
            {
                dbPlayer.SendNewNotification(
                     "Hier können nur Flugzeuge tanken!");
                return;
            }


            if (!FuelStationModule.AirportFuels.Contains(fuel.Id) && FuelStationModule.AirVehicleDataGroups.Contains(dbVehicle.Data.ClassificationId)) // disable all others on airport fuels
            {
                dbPlayer.SendNewNotification(
                      "Hier können nur Autos tanken!");
                return;
            }

            int maxLiter = 0;
            if (fuelstationFuelAmount > vehicleFuelNeeded)
            {
                maxLiter = vehicleFuelNeeded;
            }
            else
            {
                if (isOwned)
                {
                    maxLiter = fuelstationFuelAmount;
                }
                else
                {
                    maxLiter = vehicleFuelNeeded;
                }
            }

            int fueltime = 350;
            if (dbVehicle.Data.ClassificationId == 8 || dbVehicle.Data.ClassificationId == 9)
            {
                fueltime = 60; // 5000L ~ 5 Minuten
            }

            ComponentManager.Get<TextInputBoxWindow>().Show()(dbPlayer, new TextInputBoxWindowObject() { Title = fuel.Name, Callback = "fillvehicle", CustomData = new { fuel.Price, MaxLiter = maxLiter, FuelTime = fueltime } });
        }
    }
}
