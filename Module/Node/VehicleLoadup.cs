using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nexus.Handler;
using Nexus.Module.Tuning;
using Nexus.Module.Vehicles;

namespace Nexus.Module.Node
{
    public class VehicleLoadup : Module<VehicleLoadup>
    {
        public static List<SxVehicle> afterLoadVehicles = new List<SxVehicle>();

        public override bool Load(bool reload = false)
        {
            afterLoadVehicles = new List<SxVehicle>();
            return base.Load(reload);
        }

        public void StartResyncVehicleBridges()
        {
            Task.Run(async () =>
            {
                Configurations.Configuration.Instance.CanBridgeUsed = true;
                foreach (SxVehicle sxVehicle in afterLoadVehicles)
                {
                    if (sxVehicle == null || !sxVehicle.IsValid() || sxVehicle.entity == null) continue;
                    await Task.Delay(200);// Workaround for floods
                    sxVehicle.SyncMods();
                }
            });
        }
    }
}
