using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Nexus.Module.PlayerUI.Components;
using Nexus.Module.Commands;
using Nexus.Module.Houses;
using Nexus.Module.Items;
using Nexus.Module.Menu;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.JumpPoints;
using Nexus.Module.Players.Windows;
using Nexus.Module.Schwarzgeld;

namespace Nexus.Module.Bunker
{
    public class BunkerModule : SqlModule<BunkerModule, Bunker, uint>
    {
        public static Vector3 BunkerCheckPosition = new Vector3(948.496, -3233.48, -98.2978);
        public static float BunkerCheckRange = 150;
        public static uint BunkerDimension = 100;

        public static int LimitChestSellings5Min = 3;
        public int ActualChestSellings = 0;
        public int LimitRessourceAlu = 0;
        public int LimitRessourceIron = 0;
        public int LimitRessourceBatteries = 0;

        public static int PriceMethChest = 26400;
        public static int PriceCannabisChest = 26400;
        public static int PriceWeaponChest = 22000;
        public static int PriceGoldBars = 13000;
        public static int PriceDiamonds = 5700;

        public List<BunkerOrder> RessourceOrders = new List<BunkerOrder>();

        public static Vector3 BunkerBlackMoneyWithdraw = new Vector3(948.496, -3233.48, -98.2978);
        public static Vector3 BunerDealerSellMenu = new Vector3(948.84, -3202.33, -98.2699);
        public static Vector3 RessourceOrderMenu = new Vector3(937.943, -3223.03, -98.285);
        public static Vector3 BlackMoneyContainer = new Vector3(950.164, -3237.76, -98.2995);
        public static Vector3 RessourceOrderContainer = new Vector3(942.533, -3223.54, -98.2886);

        protected override void OnLoaded()
        {
            RessourceOrders = new List<BunkerOrder>();
            ActualChestSellings = 0;
            LimitRessourceAlu = 0;
            LimitRessourceIron = 0;
            LimitRessourceBatteries = 0;
        }

        protected override string GetQuery()
        {
            return "SELECT * FROM `bunker` ORDER BY RAND() LIMIT 1;";
        }
    }
}
