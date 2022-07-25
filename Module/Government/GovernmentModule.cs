using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using GTANetworkAPI;
using MySql.Data.MySqlClient;
using Nexus.Handler;
using Nexus.Module.Assets.Hair;
using Nexus.Module.Assets.HairColor;
using Nexus.Module.Assets.Tattoo;
using Nexus.Module.Barber.Windows;
using Nexus.Module.Business;
using Nexus.Module.Chat;
using Nexus.Module.PlayerUI.Components;
using Nexus.Module.Commands;
using Nexus.Module.Configurations;
using Nexus.Module.Customization;
using Nexus.Module.Government.Menu;
using Nexus.Module.GTAN;
using Nexus.Module.Houses;
using Nexus.Module.Logging;
using Nexus.Module.Menu;
using Nexus.Module.Players;

using Nexus.Module.Players.Db;
using Nexus.Module.Tattoo.Windows;
using Nexus.Module.Teams;
using Nexus.Module.Vehicles;
using static Nexus.Module.Chat.Chats;

namespace Nexus.Module.Government
{
    public class DefconLevel
    {
        public int Level { get; set; }
        public int Caller { get; set; }
        
    }

    public sealed class GovernmentModule : Module<GovernmentModule>
    {
        public static DefconLevel Defcon = null;

        public bool DefconManualSetted = false;

        public static Vector3 ComputerBuero1Pos = new Vector3(-531.384, -192.301, 38.2224);
        public static Vector3 ComputerBuero2Pos = new Vector3(-539.723, -177.858, 38.2224);

        //cleanup

        public override void OnPlayerLoadData(DbPlayer dbPlayer, MySqlDataReader reader)
        {
            Console.WriteLine("Fisch");
            dbPlayer.EconomyIndex = EconomyIndex.Low;

            int moneySum = dbPlayer.bank_money[0] + dbPlayer.money[0];

            if(dbPlayer.IsMemberOfBusiness())
            {
                Business.Business.Member memberShip = dbPlayer.BusinessMembership;

                if(memberShip.Owner)
                {
                    moneySum += dbPlayer.ActiveBusiness.Money;
                }
            }

            // SELECT SUM(vd.price) FROM vehicles v LEFT JOIN vehicledata vd ON v.model = vd.id WHERE v.owner = 1 
            using (var keyConn = new MySqlConnection(Configuration.Instance.GetMySqlConnection()))
            using (var keyCmd = keyConn.CreateCommand())
            {
                keyConn.Open();
                keyCmd.CommandText = $"SELECT SUM(vd.price) FROM vehicles v LEFT JOIN vehicledata vd ON v.model = vd.id WHERE v.owner = '{dbPlayer.Id}';";
                using (var vehicleSumReader = keyCmd.ExecuteReader())
                {
                    if (vehicleSumReader.HasRows)
                    {
                        while (vehicleSumReader.Read())
                        {
                            moneySum += vehicleSumReader.GetInt32(0);
                        }
                    }
                }
            }
            Console.WriteLine("GovernmentModule");

            House house = HouseModule.Instance.GetByOwner(dbPlayer.Id);
            if(house != null)
            {
                moneySum += house.Price;
            }

            if(moneySum > 50000000)
            {
                dbPlayer.EconomyIndex = EconomyIndex.Jeff;
            }
            else if (moneySum > 25000000)
            {
                dbPlayer.EconomyIndex = EconomyIndex.Superrich;
            }
            else if (moneySum > 10000000)
            {
                dbPlayer.EconomyIndex = EconomyIndex.Rich;
            }
            else if (moneySum > 5000000)
            {
                dbPlayer.EconomyIndex = EconomyIndex.Mid;
            }
            else dbPlayer.EconomyIndex = EconomyIndex.Low;



        }

        protected override bool OnLoad()
        {
            Defcon = new DefconLevel() { Level = 5, Caller = 0};

            using (var conn = new MySqlConnection(Configuration.Instance.GetMySqlConnection()))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = $"SELECT * FROM defcon ORDER BY date DESC LIMIT 1;";
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Defcon = new DefconLevel() { Level = reader.GetInt32("level"), Caller = reader.GetInt32("caller") };
                        }
                    }
                }
                conn.Close();
            }
            
            Task.Run(async () => { await SetDefcon(Defcon.Level, Defcon.Caller); });

            // Load Menu

            MenuManager.Instance.AddBuilder(new GovComputerMenuBuilder());

            return base.OnLoad();
        }

        public override void OnFiveMinuteUpdate()
        {
            DateTime time = DateTime.Now;
            if(time.Hour < 16)
            {
                if(Defcon.Level > 3)
                {
                    Task.Run(async () => { await SetDefcon(3, 0); });
                }
            }

            if(time.Hour >= 16 && Defcon.Level < 4 && !DefconManualSetted)
            {
                Task.Run(async () => { await SetDefcon(4, 0); });
            }
        }

        public async Task SetDefcon(int Level, int CallerId, bool save = true)
        {

            Defcon.Caller = CallerId;
            Defcon.Level = Level;
            
            if(save) MySQLHandler.ExecuteAsync($"INSERT INTO defcon (`level`, `caller`) VALUES ('{Level}', '{CallerId}')");

            await Chats.SendGlobalMessage($"Regierungsmitteilung: Die aktuelle Defcon Stufe {Level} wurde ausgerufen!", COLOR.LIGHTBLUE, ICON.GOV);
        }
        
        [CommandPermission(PlayerRankPermission = true)]
        [Command]
        public void Commandsetdefcon(Player player, string level)
        {
            var iPlayer = player.GetPlayer();

            if (iPlayer == null || !iPlayer.IsValid()) return;

            if (!Int32.TryParse(level, out int defcons)) return;

            if (defcons <= 0 || defcons > 5) return;

            if(iPlayer.GovLevel.ToLower() == "b" || iPlayer.GovLevel.ToLower() == "a")
            {
                Task.Run(async () => { await SetDefcon(defcons, (int)iPlayer.Id); });
                DefconManualSetted = true;
                return;
            }
        }

    }
}