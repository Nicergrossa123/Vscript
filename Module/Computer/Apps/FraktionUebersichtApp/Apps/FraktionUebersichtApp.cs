﻿using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using Nexus.Module.PlayerUI.Apps;
using Nexus.Module.Computer.Apps.FahrzeugUebersichtApp;
using Nexus.Module.Computer.Apps.VehicleImpoundApp;
using Nexus.Module.Configurations;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Newtonsoft.Json;
using Nexus.Module.Logging;
using Nexus.Module.Teams;

namespace Nexus.Module.Computer.Apps.FraktionUebersichtApp.Apps
{
    public class FraktionUebersichtApp : SimpleApp
    {
        public FraktionUebersichtApp() : base("FraktionListApp") { }


        [RemoteEvent]
        public void requestFraktionMembers(Player Player)
        {
            DbPlayer p_DbPlayer = Player.GetPlayer();
            if (p_DbPlayer == null || !p_DbPlayer.IsValid())
                return;
            if (p_DbPlayer.TeamId == 0) return;
            List<Frakmember> frakMembers = new List<Frakmember>();

            using (MySqlConnection conn = new MySqlConnection(Configuration.Instance.GetMySqlConnection()))
            using (MySqlCommand cmd = conn.CreateCommand())
            {
                conn.Open();

                cmd.CommandText = "SELECT player.id, player.name, player.rang, player.fgehalt, player_rights.* FROM player INNER JOIN player_rights ON player_rights.accountid = player.id WHERE team = @team GROUP BY player.id ORDER BY rang DESC";
                cmd.Parameters.AddWithValue("@team", p_DbPlayer.TeamId);
                cmd.Prepare();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int gehalt = reader.GetInt32("fgehalt");
                            if (p_DbPlayer.Team.HasDuty && TeamModule.Instance.Get(p_DbPlayer.TeamId).Salary[reader.GetInt32("rang")] > 0)
                            {
                                gehalt = TeamModule.Instance.Get(p_DbPlayer.TeamId).Salary[reader.GetInt32("rang")];
                            }

                            Frakmember overview = new Frakmember
                            {
                                Id = reader.GetUInt32("id"),
                                Name = reader.GetString("name"),
                                Rang = reader.GetUInt32("rang"),
                                Payday = gehalt,
                                Title = reader.GetString("title"),
                                Bank = reader.GetInt32("r_bank") == 1,
                                Manage = reader.GetInt32("r_manage") >= 1,
                                Storage = reader.GetInt32("r_inventory") == 1
                            };
                            frakMembers.Add(overview);
                        }
                    }
                }
            }

            var membersManageObject = new FrakMembersObject() { Frakmembers = frakMembers, ManagePermission = p_DbPlayer.TeamRankPermission.Manage >= 1 ? true : false };
            var membersJson = JsonConvert.SerializeObject(membersManageObject);

            //Logger.GlobalDebug(membersJson);
            //Console.WriteLine(membersJson);

            if (!string.IsNullOrEmpty(membersJson))
            {
                TriggerEvent(p_DbPlayer.Player, "responseMembers", membersJson, p_DbPlayer.Team.HasDuty);
            }
        }
    }
}