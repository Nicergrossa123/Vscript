﻿using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Nexus.Module.PlayerUI.Apps;
using Nexus.Module.Houses;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Logger = Nexus.Module.Logging.Logger;

namespace Nexus.Module.Computer.Apps.HouseApp.Apps
{
    public class HouseVehicleList : SimpleApp
    {

        public HouseVehicleList() : base("HouseVehicleList")
        {

        }

        [RemoteEvent]
        public void requestHouseVehicles(Player Player)
        {
            DbPlayer dbPlayer = Player.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid())
                return;

            if (dbPlayer.ownHouse[0] == 0)
            {
                dbPlayer.SendNewNotification("Du besitzt kein Haus.");
                return;
            }
            House house = HouseModule.Instance.GetByOwner(dbPlayer.Id);
            if (house == null) return;

            TriggerEvent(Player, "responseHouseVehicles", NAPI.Util.ToJson(HouseAppFunctions.GetVehiclesForHouseByPlayer(dbPlayer, house)));
        }

        [RemoteEvent]
        public void dropHouseVehicle(Player Player, int vehicleId)
        {
            DbPlayer dbPlayer = Player.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid())
                return;

            if (dbPlayer.ownHouse[0] == 0) return;

            House xHouse = HouseModule.Instance.GetByOwner(dbPlayer.Id);
            if (xHouse == null) return;

            MySQLHandler.ExecuteAsync($"UPDATE vehicles SET garage_id = 1 WHERE id = '{vehicleId}' AND garage_id = '{xHouse.GarageId}';");
            dbPlayer.SendNewNotification($"Du hast das Fahrzeug mit der ID {vehicleId} erfolgreich aus deiner Hausgarage entfernt.");
        }
    }
}
