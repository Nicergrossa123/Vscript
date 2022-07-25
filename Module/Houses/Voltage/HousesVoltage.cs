
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using GTANetworkAPI;
using Nexus.Module;
using Nexus.Module.Configurations;
using Nexus.Module.Houses;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.PlayerTask;
using Nexus.Module.Vehicles.Garages;
using Nexus.Module.Items;
using System;
using Nexus.Module.Schwarzgeld;
using Nexus.Module.Logging;

namespace Nexus.Module.Houses
{
    public class HousesVoltage : Loadable<uint>
    {
        public uint Id { get; set; }
       
        public Vector3 Position { get; set; }
        public List<uint> DetectedHouses { get; set; }
    
        public HousesVoltage(MySqlDataReader reader) : base(reader)
        {
            Id = reader.GetUInt32("id");
         
            Position = new Vector3(reader.GetFloat("pos_x"), reader.GetFloat("pos_y"), reader.GetFloat("pos_z"));

            DetectedHouses = new List<uint>();

            //if(Configurations.Configuration.Instance.DevMode) Spawners.Markers.Create(1, Position, new Vector3(), new Vector3(), 2.0f, 255, 255, 0, 0);

        }

        public override uint GetIdentifier()
        {
            return Id;
        }
    }
}