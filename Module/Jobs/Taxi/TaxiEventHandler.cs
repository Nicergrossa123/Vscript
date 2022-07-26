﻿using System;
using GTANetworkAPI;
using Nexus.Module.Players;

namespace Nexus.Module.Jobs.Taxi
{
    public class TaxiEventHandler : Script
    {
        [RemoteEvent]
        public void resultTaxometer(Player Player, double distance, int price)
        {
            var iPlayer = Player.GetPlayer();
            if (iPlayer == null || !iPlayer.IsValid()) return;

            iPlayer.SendNewNotification(
                "Taxometer lief fuer " + distance + "km. Gesamtpreis: " + Math.Round(distance * price) +
                "$");
        }
    }
}