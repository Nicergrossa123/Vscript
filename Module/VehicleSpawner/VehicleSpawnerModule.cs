﻿using System;
using System.Threading.Tasks;
using GTANetworkAPI;
using Nexus.Handler;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Vehicles;

namespace Nexus.Module.VehicleSpawner
{
    public sealed class VehicleSpawnerModule : Module<VehicleSpawnerModule>
    {
        public override void OnPlayerDisconnected(DbPlayer dbPlayer, string reason)
        {
            if(dbPlayer.Player.IsInVehicle)
            {
                if(dbPlayer.Player.VehicleSeat == -1) // driver
                {
                    SxVehicle sxVehicle = dbPlayer.Player.Vehicle.GetVehicle();
                    if(sxVehicle != null && sxVehicle.GetSpeed() > 0 && sxVehicle.entity.Position.Z > 20.0f) 
                    {
                        // Parke helis & flugzeuge ein...
                        if(sxVehicle.Data.ClassificationId == 9 || sxVehicle.Data.ClassificationId == 8)
                        {
                            Task.Run(async () =>
                            {
                                await Task.Delay(60000);
                                if (sxVehicle != null && sxVehicle.entity.IsSeatFree(-1))
                                {
                                    if (sxVehicle.IsPlayerVehicle()) sxVehicle.SetPrivateCarGarage(1);
                                    if (sxVehicle.IsTeamVehicle()) sxVehicle.SetTeamCarGarage(true);
                                    else sxVehicle.entity.DeleteVehicle();
                                }
                            });
                        }
                    }
                }
            }
        }

        public override void OnMinuteUpdate()
        {
            foreach (SxVehicle sxVehicle in VehicleHandler.Instance.GetAllVehicles())
            {
                if (sxVehicle == null || !sxVehicle.IsValid()) continue;

                if (sxVehicle.IsPlayerVehicle() || sxVehicle.IsTeamVehicle())
                {
                    if (sxVehicle.entity.HasData("lastSavedPos"))
                    {
                        if (sxVehicle.entity == null) continue;
                        Vector3 lastSavedPos = (Vector3)sxVehicle.entity.GetData<Vector3>("lastSavedPos");
                        if (lastSavedPos.DistanceTo(sxVehicle.entity.Position) > 5.0f)
                        {
                            SaveVehiclePosition(sxVehicle);
                        }
                    }
                    else
                    {
                        SaveVehiclePosition(sxVehicle);
                    }
                }
            }
        }

        public void SaveVehiclePosition(SxVehicle sxVehicle)
        {
            string x = sxVehicle.entity.Position.X.ToString().Replace(",", ".");
            string y = sxVehicle.entity.Position.Y.ToString().Replace(",", ".");
            string z = sxVehicle.entity.Position.Z.ToString().Replace(",", ".");
            string rotation = sxVehicle.entity.Rotation.Z.ToString().Replace(",", ".");
            
            if (sxVehicle.databaseId == 0) return;
            if (sxVehicle.IsTeamVehicle())
            {
                MySQLHandler.ExecuteAsync($"UPDATE fvehicles SET pos_x = '{x}', pos_y = '{y}', pos_z = '{z}', `fuel` = '{sxVehicle.fuel}', `zustand` = '{Convert.ToInt32(sxVehicle.entity.Health)}', `km` = '{Convert.ToInt32(sxVehicle.Distance)}', `rotation` = '{rotation}' WHERE id = '{sxVehicle.databaseId}' AND team = '{sxVehicle.teamid}'");
            }
            else if (sxVehicle.IsPlayerVehicle())
            {
                MySQLHandler.ExecuteAsync($"UPDATE vehicles SET pos_x = '{x}', pos_y = '{y}', pos_z = '{z}', `fuel` = '{sxVehicle.fuel}', `zustand` = '{Convert.ToInt32(sxVehicle.entity.Health)}', `km` = '{Convert.ToInt32(sxVehicle.Distance)}', `heading` = '{rotation}' WHERE id = '{sxVehicle.databaseId}' AND owner = '{sxVehicle.ownerId}'");
            }
            
            sxVehicle.entity.SetData("lastSavedPos", sxVehicle.entity.Position);
        }
    }
}