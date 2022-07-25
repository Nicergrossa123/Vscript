using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GTANetworkAPI;
using MySql.Data.MySqlClient;
using Nexus.Handler;
using Nexus.Module.Assets.Beard;
using Nexus.Module.Assets.Hair;
using Nexus.Module.Assets.HairColor;
using Nexus.Module.Barber.Windows;
using Nexus.Module.PlayerUI.Components;
using Nexus.Module.Commands;
using Nexus.Module.Configurations;
using Nexus.Module.GTAN;
using Nexus.Module.Logging;
using Nexus.Module.Players;

using Nexus.Module.Players.Db;
using Nexus.Module.Teams;
using Nexus.Module.Teams.Shelter;
using Nexus.Module.Vehicles;
using Nexus.Module.Vehicles.Data;

namespace Nexus.Module.VehicleTax
{
    public sealed class VehicleTaxModule : Module<VehicleTaxModule>
    {
        public override void OnPlayerLoadData(DbPlayer dbPlayer, MySqlDataReader reader)
        {
            dbPlayer.VehicleTaxSum = reader.GetInt32("tax_sum");

            Console.WriteLine("VehicleTaxModule");

        }

        public override void OnFiveMinuteUpdate()
        {
            Dictionary<uint, int> taxes = new Dictionary<uint, int>();

            foreach (var VARIABLE in TeamModule.Instance.GetAll().Where(x => x.Value.IsGangsters()).ToList())
            {
                taxes.Add(VARIABLE.Key, 0);
            }
            //Console.WriteLine("LALALALLALALALA " + taxes.Count.ToString());
            foreach (SxVehicle sxVehicle in VehicleHandler.SxVehicles.Values.ToList().Where(x => x.IsValid() && x.Registered == true).ToList())
            {
                if (sxVehicle.databaseId <= 0)continue;
                if (sxVehicle.IsPlayerVehicle())
                {
                    // Try to get Owner on server
                    DbPlayer dbPlayer = Players.Players.Instance.FindPlayerById(sxVehicle.ownerId);
                    if(dbPlayer != null && dbPlayer.IsValid())
                    {
                        dbPlayer.VehicleTaxSum += sxVehicle.Data.Tax/12; // Steuern / 60
                    }
                }
                else
                {
                    Team team = sxVehicle.Team;
                    if (!team.IsGangsters()) continue;
                    var newValue = taxes.GetValueOrDefault(team.Id) + sxVehicle.Data.Tax / 12;
                    taxes.Remove(team.Id);
                    taxes.Add(team.Id, newValue);
                }
            }

            foreach (var VARIABLE in taxes.Keys.ToList())
            {
                try
                {
                    TeamShelter teamShelter = TeamShelterModule.Instance.Get(VARIABLE);
                    if (teamShelter == null) continue;
                    var cost = taxes.GetValueOrDefault(VARIABLE) + Main.GetTeamVehicleTaxes(VARIABLE) / 24;
                    teamShelter.TakeMoney(cost);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }
            }


            foreach (DbPlayer dbPlayer in Players.Players.Instance.GetValidPlayers())
            {
                dbPlayer.VehicleTaxSum += GetPlayerVehicleTaxesForGarages(dbPlayer) / 24; // hälfte der Steuern wenn in garage
            }
        }

        public static int GetPlayerVehicleTaxesForGarages(DbPlayer iPlayer)
        {
            int tax = 0;

            string query = $"SELECT * FROM `vehicles` WHERE `owner` = '{iPlayer.Id}' AND `inGarage` = '1' AND `registered` = '1';";

            using (var conn = new MySqlConnection(Configuration.Instance.GetMySqlConnection()))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @query;
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var modelId = reader.GetUInt32("model");
                            var data = VehicleDataModule.Instance.GetDataById(modelId);
                            if (data == null) continue;
                            tax = tax + data.Tax;
                        }
                    }
                }
            }
            return tax;
        }
    }
}