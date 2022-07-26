﻿using System;
using System.Collections.Generic;
using System.Text;
using Nexus.Handler;
using Nexus.Module.Items;
using Nexus.Module.Teams;
using Nexus.Module.Vehicles;
using Nexus.Module.Vehicles.Data;

namespace Nexus.Module.VehicleSpawner
{
    class FraktionsVehicleModule : SqlModule<FraktionsVehicleModule, FraktionsVehicle, uint>
    {
        public override Type[] RequiredModules()
        {
            return new[] { typeof(ItemsModule), typeof(ItemModelModule), typeof(VehicleDataModule) };
        }

        protected override string GetQuery()
        {
            return "SELECT * FROM `fvehicles` WHERE !(`pos_x` = '0') AND `inGarage` = '0' AND `lastGarage` > 0;";
        }

        protected override void OnItemLoaded(FraktionsVehicle fvehicle)
        {
            Logging.Logger.Debug("model " + fvehicle.Model);
            var data = VehicleDataModule.Instance.GetDataById((uint)fvehicle.Model);
            if (data == null) return;
            if (data.Disabled) return;

            SxVehicle xVeh = VehicleHandler.Instance.CreateServerVehicle(data.Id, fvehicle.Registered,
                                    fvehicle.Position, fvehicle.Rotation,
                                    fvehicle.Color1, fvehicle.Color2, 0, fvehicle.GpsTracker, true, true,
                                    fvehicle.TeamId, TeamModule.Instance.Get(fvehicle.TeamId).ShortName,
                                    fvehicle.Id, 0, 0, fvehicle.Fuel,
                                    VehicleHandler.MaxVehicleHealth, fvehicle.Tuning, "", 0, ContainerManager.LoadContainer(fvehicle.Id, ContainerTypes.FVEHICLE), fvehicle.Plate, false, false, fvehicle.WheelClamp, fvehicle.AlarmSystem, fvehicle.lastGarage, false, fvehicle.CarSellPrice);
            Logging.Logger.Debug($"FVEHICLE {fvehicle.Model} {TeamModule.Instance.Get(fvehicle.TeamId).ShortName} loaded");

            xVeh.SetTeamCarGarage(false);
        }
    }
}
