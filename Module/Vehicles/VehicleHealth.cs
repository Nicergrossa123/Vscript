using GTANetworkAPI;
using System;
using System.Linq;
using System.Threading.Tasks;
using Nexus.Handler;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;

namespace Nexus.Module.Vehicles
{
    public static class VehicleHealth
    {
        // repair kit id

        public static void Repair(this SxVehicle vehicle)
        {
            NAPI.Task.Run(async() => {

                vehicle.RepairState = true;
                vehicle.entity.Repair();

                //set Health to max
                vehicle.entity.Health = VehicleHandler.MaxVehicleHealth;

                // Resync max speed cap
                try
                {
                    await Task.Delay(500);

                    if (vehicle != null && vehicle.Data != null && vehicle.Data.MaxSpeed > 0)
                    {
                        foreach (DbPlayer occu in vehicle.Occupants.Values.ToList())
                        {
                            if (occu != null && occu.IsValid())
                            {
                                occu.Player.TriggerEvent("setNormalSpeed", vehicle.entity, vehicle.Data.MaxSpeed);
                            }
                        }
                    }

                    await Task.Delay(1500);

                    if (vehicle != null && vehicle.Data != null && vehicle.Data.MaxSpeed > 0)
                    {
                        foreach (DbPlayer occu in vehicle.Occupants.Values.ToList())
                        {
                            if (occu != null && occu.IsValid())
                            {
                                occu.Player.TriggerEvent("setNormalSpeed", vehicle.entity, vehicle.Data.MaxSpeed);
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    Logging.Logger.Crash(e);
                }
            });
        }

        public static void SetHealth(this SxVehicle vehicle, float health)
        {
            vehicle.RepairState = true;
            vehicle.entity.Health = health;
        }
    }
}