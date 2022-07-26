﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GTANetworkAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Newtonsoft.Json;
using Nexus.Module.PlayerUI.Apps;
using Nexus.Module.Crime;
using Nexus.Module.Crime.PoliceAkten;
using Nexus.Module.Houses;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Logger = Nexus.Module.Logging.Logger;

namespace Nexus.Module.Computer.Apps.PoliceAktenSearchApp
{
    public class PoliceListProgressApp : SimpleApp
    {
        public PoliceListProgressApp() : base("PoliceListProgressApp")
        {
        }

        [RemoteEvent]
        public async void requestCrimeProgress(Player p_Player, string p_Name)
        {
            var dbPlayer = p_Player.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid()) return;

            if (!MySQLHandler.IsValidNoSQLi(dbPlayer, p_Name)) return;

            var foundPlayer = Players.Players.Instance.FindPlayer(p_Name);
            if (foundPlayer == null || !foundPlayer.IsValid()) return;

            Logging.Logger.Debug("requestCrimeProgress");

            List<CrimeHistoryJson> jsonCrimeHistory = new List<CrimeHistoryJson>();

            foreach(CrimePlayerHistory cph in foundPlayer.CrimeHistories)
            {
                jsonCrimeHistory.Add(new CrimeHistoryJson() { Date = $"{cph.Date.ToString("dd / MM / yyyy")} - {cph.Date.ToString("HH:mm")} Uhr", Text = cph.Crimes });
            }

            ResponseCrimeHistoryObject responseCrimeHistoryObject = new ResponseCrimeHistoryObject();
            responseCrimeHistoryObject.crimeHistoryJsons = jsonCrimeHistory;

            TriggerEvent(p_Player, "responseCrimeProgress", NAPI.Util.ToJson(responseCrimeHistoryObject));
            Logging.Logger.Debug(NAPI.Util.ToJson(responseCrimeHistoryObject));
        }

    }
  
    public class CrimeHistoryJson
    {
        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
    }

    public class ResponseCrimeHistoryObject
    {
        [JsonProperty(PropertyName = "vv")]
        public List<CrimeHistoryJson> crimeHistoryJsons;
    }
}

