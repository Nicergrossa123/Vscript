﻿using GTANetworkAPI;
using Nexus.Module.Chat;
using Nexus.Module.FIB;
using Nexus.Module.Players.Db;
using Nexus.Module.Swat;

namespace Nexus.Module.Players
{
    public static class PlayerLicenses
    {
        //ToDo: Add Custom Window

        public static void ShowLicenses(this DbPlayer dbPlayer, Player destinationPlayer)
        {
            string l_Name = dbPlayer.GetName();
            if ((dbPlayer.IsSwatDuty() || (dbPlayer.IsACop() && dbPlayer.IsInDuty())) && !dbPlayer.IsUndercover())
            {
                l_Name = "Unbekannt";
            }

            destinationPlayer.TriggerEvent("showLicense", l_Name, dbPlayer.Lic_FirstAID[0], dbPlayer.Lic_Gun[0], dbPlayer.Lic_Car[0], dbPlayer.Lic_LKW[0], dbPlayer.Lic_Bike[0], dbPlayer.Lic_Boot[0], dbPlayer.Lic_PlaneA[0], dbPlayer.Lic_PlaneB[0], dbPlayer.Lic_Taxi[0], dbPlayer.Lic_Transfer[0], 0, dbPlayer.marryLic);
        }
    }
}