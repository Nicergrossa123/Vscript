using GTANetworkAPI;

namespace Nexus.Module.Vehicles
{
    public static class VehicleDelete
    {
        public static void DeleteVehicle(this Vehicle vehicle)
        {
            NAPI.Task.Run(() =>
            {
                vehicle.Delete();
            });
        }
    }
}