﻿using GTANetworkAPI;
using System.Threading.Tasks;
using Nexus.Handler;
using Nexus.Module.PlayerUI.Apps;
using Nexus.Module.Commands;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Vehicles;
using static Nexus.Module.Computer.Apps.SupportVehicleApp.SupportVehicleFunctions;

namespace Nexus.Module.Computer.Apps.SupportVehicleApp.Apps
{
    class SupportVehicleProfile : SimpleApp
    {
        public SupportVehicleProfile() : base("SupportVehicleProfile") { }

        [RemoteEvent]
        public async void requestVehicleData(Player Player, int id)
        {
            
                DbPlayer iPlayer = Player.GetPlayer();
                if (iPlayer == null || !iPlayer.IsValid()) return;

                VehicleCategory category = VehicleCategory.ID;
                var vehicleData = SupportVehicleFunctions.GetVehicleData(category, id);

                var vehicleDataJson = NAPI.Util.ToJson(vehicleData);
                TriggerEvent(Player, "responseVehicleData", vehicleDataJson);
            
        }

        [RemoteEvent]
        public async void SupportSetGarage(Player Player, uint vehicleId)
        {
            
                DbPlayer iPlayer = Player.GetPlayer();
                if (iPlayer == null || !iPlayer.IsValid()) return;
                if (!iPlayer.CanAccessMethod("removeveh")) return;

                SxVehicle Vehicle = VehicleHandler.Instance.GetByVehicleDatabaseId(vehicleId);
                if (Vehicle == null) return;

                if (Vehicle.IsPlayerVehicle())
                {
                    Vehicle.SetPrivateCarGarage(1);
                    iPlayer.SendNewNotification("Fahrzeug wurde in die Garage gesetzt!", title: "ADMIN", notificationType: PlayerNotification.NotificationType.ADMIN);
                }
                else
                {
                    iPlayer.SendNewNotification("Fahrzeug ist kein privat Fahrzeug!", title: "ADMIN", notificationType: PlayerNotification.NotificationType.ADMIN);
                }
            
        }

        [RemoteEvent]
        public async void SupportGoToVehicle(Player Player, uint vehicleId)
        {
            
                DbPlayer iPlayer = Player.GetPlayer();
                if (iPlayer == null || !iPlayer.IsValid()) return;
                if (!iPlayer.CanAccessMethod("removeveh")) return;

                SxVehicle Vehicle = VehicleHandler.Instance.GetByVehicleDatabaseId(vehicleId);
                if (Vehicle == null) return;

                Vector3 pos = Vehicle.entity.Position;

                if (Player.IsInVehicle)
                {
                    Player.Vehicle.Position = pos;
                }
                else
                {
                    Player.SetPosition(pos);
                }

                iPlayer.SendNewNotification($"Sie haben sich zu Fahrzeug {Vehicle.databaseId} teleportiert!", title: "ADMIN", notificationType: PlayerNotification.NotificationType.ADMIN);
            
        }
    }
}
