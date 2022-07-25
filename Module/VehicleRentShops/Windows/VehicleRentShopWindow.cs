using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Nexus.Module.PlayerUI.Windows;
using Nexus.Module.Logging;
using Nexus.Module.Players.Db;
using Nexus.Module.Players;
using Nexus.Module.Items;
using Nexus.Module.Voice;
using Nexus.Module.Chat;
using Nexus.Module.Business.NightClubs;
using System.Threading.Tasks;
using Nexus.Module.Schwarzgeld;
using Nexus.Module.NSA;
using Nexus.Module.Teams;
using Nexus.Module.Events.CWS;
using Nexus.Module.Teams.Shelter;
using Nexus.Handler;
using Nexus.Module.Vehicles.Data;

namespace Nexus.Module.VehicleRentShops.Windows
{
    
    public class VehicleRentShopWindow : Window<Func<DbPlayer, VehicleRentShop, bool>>
    {
        private class ShowEvent : Event
        {
            [JsonProperty(PropertyName = "vehiclerent")] private VehicleRentShop vehiclerent { get; }

            public ShowEvent(DbPlayer dbPlayer, VehicleRentShop vehicleRentShop) : base(dbPlayer)
            {
                vehicleRentShop.ActualizeLeftAmount();

                vehiclerent = vehicleRentShop;
            }
        }

        public VehicleRentShopWindow() : base("VehicleRent")
        {
        }

        public override Func<DbPlayer, VehicleRentShop, bool> Show()
        {
            return (player, vehiclerent) => OnShow(new ShowEvent(player, vehiclerent));
        }

        [RemoteEvent]
        public void VehicleRentAction(Player Player, uint vehicleRentShopId, uint vehicleRentShopItemId)
        {
            try
            {
                DbPlayer iPlayer = Player.GetPlayer();

                if (iPlayer == null || !iPlayer.IsValid()) return;

                VehicleRentShop vehicleRentShop = VehicleRentShopModule.Instance.GetAll().Values.Where(s => s.Id == vehicleRentShopId && s.Position.DistanceTo(iPlayer.Player.Position) < 5.0f).FirstOrDefault();


                if (vehicleRentShop != null)
                {
                    vehicleRentShop.ActualizeLeftAmount();

                    if (vehicleRentShop.FreeToRent <= 0)
                    {
                        iPlayer.SendNewNotification("Keine freien Fahrzeuge zur Verfügung!");
                        return;
                    }

                    SxVehicle sxVehicle = VehicleHandler.Instance.GetJobVehicles().Where(js => js.ownerId == iPlayer.Id && js.jobid == ((int)VehicleRentShopModule.FakeJobVehicleRentShopId + (int)vehicleRentShop.Id)).FirstOrDefault();
                    if (sxVehicle != null)
                    {
                        iPlayer.SendNewNotification("Sie haben sich hier bereits ein Fahrzeug gemietet!");
                        return;
                    }

                    VehicleRentShopItem vehicleRentShopItem = vehicleRentShop.ShopItems.Where(i => i.Id == vehicleRentShopItemId).FirstOrDefault();
                    if (vehicleRentShopItem != null)
                    {
                        // Get Spawn
                        VehicleRentShopSpawn vehicleRentShopSpawn = vehicleRentShop.GetFreeSpawnPosition();
                        if (vehicleRentShopSpawn == null)
                        {
                            iPlayer.SendNewNotification("Kein Ausparkpunkt verfügbar!");
                            return;
                        }

                        if (!iPlayer.TakeMoney(vehicleRentShopItem.Price))
                        {
                            iPlayer.SendNewNotification(MSG.Money.NotEnoughMoney(vehicleRentShopItem.Price));
                            return;
                        }

                        // Spawn Vehicle and set vehicle data
                        SxVehicle rentVeh = VehicleHandler.Instance.CreateServerVehicle(vehicleRentShopItem.VehicleModelId, false, vehicleRentShopSpawn.Position, vehicleRentShopSpawn.Heading, -1, -1, 0, true, true, false, 0, iPlayer.GetName(), 0, ((int)VehicleRentShopModule.FakeJobVehicleRentShopId + (int)vehicleRentShop.Id), iPlayer.Id, plate: "Miet KFZ");

                        if (rentVeh != null && !VehicleRentShopModule.Instance.ShopRentsVehicles.ContainsKey(vehicleRentShop.Id))
                        {
                            VehicleRentShopModule.Instance.ShopRentsVehicles[vehicleRentShop.Id].Add(rentVeh);
                        }

                        iPlayer.SendNewNotification($"Sie haben sich für $ {vehicleRentShopItem.Price} ein {rentVeh.GetName()} gemietet!");
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Crash(e);
            }
        }
    }
}
