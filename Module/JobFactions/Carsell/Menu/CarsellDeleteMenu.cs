using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nexus.Handler;
using Nexus.Module.PlayerUI.Components;
using Nexus.Module.Configurations;
using Nexus.Module.Menu;
using Nexus.Module.NSA.Observation;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.Windows;
using Nexus.Module.Telefon.App;
using Nexus.Module.Vehicles.Data;

namespace Nexus.Module.Carsell.Menu
{
    public class CarsellDeleteMenuBuilder : MenuBuilder
    {
        public CarsellDeleteMenuBuilder() : base(PlayerMenu.CarsellDeleteMenu)
        {

        }

        public override Module.Menu.Menu Build(DbPlayer DbPlayer)
        {
            var l_Menu = new Module.Menu.Menu(Menu, "Fahrzeug entfernen");
            l_Menu.Add($"Schließen");

            using (var conn = new MySqlConnection(Configuration.Instance.GetMySqlConnection()))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = $"SELECT * FROM fvehicles WHERE team = '{DbPlayer.TeamId}';";
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            VehicleData vehData = VehicleDataModule.Instance.GetDataById(reader.GetUInt32("model"));
                            if (vehData == null) continue;
                            
                            l_Menu.Add($"{(vehData.mod_car_name.Length <= 0 ? vehData.Model : vehData.mod_car_name)}");
                        }
                    }
                }
                conn.Close();
            }

            return l_Menu;
        }

        public override IMenuEventHandler GetEventHandler()
        {
            return new EventHandler();
        }

        private class EventHandler : IMenuEventHandler
        {
            public bool OnSelect(int index, DbPlayer iPlayer)
            {
                if(index == 0)
                {
                    MenuManager.DismissCurrent(iPlayer);
                    return true;
                }

                int idx = 1;

                using (var conn = new MySqlConnection(Configuration.Instance.GetMySqlConnection()))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = $"SELECT * FROM fvehicles WHERE team = '{iPlayer.TeamId}';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                VehicleData vehData = VehicleDataModule.Instance.GetDataById(reader.GetUInt32("model"));
                                if (vehData == null) continue;
                                
                                if (index == idx)
                                {
                                    if (reader.GetInt32("inGarage") == 0)
                                    {
                                        iPlayer.SendNewNotification($"Fahrzeug ist derzeit ausgeparkt!");
                                        return true;
                                    }

                                    MySQLHandler.ExecuteAsync($"DELETE FROM `fvehicles` WHERE id = '{reader.GetUInt32("id")}';");
                                    iPlayer.SendNewNotification($"Fahrzeug {(vehData.mod_car_name.Length <= 0 ? vehData.Model : vehData.mod_car_name)} wurde entfernt!");
                                    return true;
                                }
                                idx++;
                            }
                        }
                    }
                    conn.Close();
                }

                MenuManager.DismissCurrent(iPlayer);
                return true;
            }
        }
    }
}
