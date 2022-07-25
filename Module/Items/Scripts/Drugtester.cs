﻿using System;
using System.Linq;
using System.Threading.Tasks;
using GTANetworkAPI;
using Nexus.Handler;
using Nexus.Module.Chat;
using Nexus.Module.Events.Halloween;
using Nexus.Module.Gangwar;
using Nexus.Module.Laboratories;
using Nexus.Module.Meth;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.PlayerAnimations;
using Nexus.Module.Vehicles;

namespace Nexus.Module.Items.Scripts
{
    public static partial class ItemScript
    {
        public static async Task<bool> DrugtestAir(DbPlayer iPlayer, ItemModel ItemData)
        {
            if (iPlayer.Player.IsInVehicle || iPlayer.TeamId != (int)teams.TEAM_FIB) return false;
        
            if (iPlayer.HasData("lastDrugAirUsed"))
            {
                DateTime lastUsed = iPlayer.GetData("lastDrugAirUsed");
                if(lastUsed.AddMinutes(1) > DateTime.Now)
                {
                    iPlayer.SendNewNotification("Es kann nur eine Messung innerhalb 1 Minuten durchgeführt werden!");
                    return true;
                }
                else
                    iPlayer.SetData("lastDrugAirUsed", DateTime.Now);
            }
            else
                iPlayer.SetData("lastDrugAirUsed", DateTime.Now);

            Chat.Chats.sendProgressBar(iPlayer, 30000);
            await Task.Delay(30000);

            foreach (SxVehicle sxvehicle in MethModule.CookingVehicles.ToList())
            {
                if (sxvehicle == null || !sxvehicle.IsValid()) continue;
                float Distance = iPlayer.Player.Position.DistanceTo(sxvehicle.entity.Position);
                if (Distance < MethModule.CamperDrugAirRange)
                {
                    // get percent
                    int percDist = 10-((int)((Distance/60) * 10));
                    int Stickstoff = Convert.ToInt32(0.78f * (100-percDist));
                    int Sauerstoff = Convert.ToInt32(0.20f * (100-percDist));
                    iPlayer.SendNewNotification($"Lufttest: Stickstoff {Stickstoff}%, Sauerstoff {Sauerstoff}%, Aldehyde {percDist}%");
                    return true;
                }
            }
            
            foreach(Methlaboratory methlaboratory in Laboratories.MethlaboratoryModule.Instance.GetAll().Values.Where(m => m.ProzessingPlayers.Count() > 0))
            {
                float Distance = iPlayer.Player.Position.DistanceTo(methlaboratory.JumpPointEingang.Position);
                float maxDistance = methlaboratory.ProzessingPlayers.Count() * MethModule.DrugLabIncreaseRange;
                if (maxDistance > 150) maxDistance = 150;
                if(Distance < maxDistance)
                {
                    // get percent
                    int percDist = 10 - ((int)((Distance / 60) * 10));
                    int Stickstoff = Convert.ToInt32(0.78 * (100 - percDist));
                    int Sauerstoff = Convert.ToInt32(0.20 * (100 - percDist));
                    iPlayer.SendNewNotification($"Lufttest: Stickstoff {Stickstoff}%, Sauerstoff {Sauerstoff}%, Aldehyde {percDist}%");
                    return true;
                }
            }

            foreach (Cannabislaboratory cannabislab in Laboratories.CannabislaboratoryModule.Instance.GetAll().Values.Where(m => m.ActingPlayers.Count > 0))
            {
                float Distance = iPlayer.Player.Position.DistanceTo(cannabislab.JumpPointEingang.Position);
                float maxDistance = cannabislab.ActingPlayers.Count() * MethModule.DrugLabIncreaseRange;
                if (maxDistance > 150) maxDistance = 150;
                if (Distance < maxDistance)
                {
                    int percDist = 10 - ((int)((Distance / 60) * 10));
                    int Stickstoff = Convert.ToInt32(0.78 * (100 - percDist));
                    int Sauerstoff = Convert.ToInt32(0.20 * (100 - percDist));
                    iPlayer.SendNewNotification($"Lufttest: Stickstoff {Stickstoff}%, Sauerstoff {Sauerstoff}%, THC-Gehalt {percDist}%");
                    return true;
                }
            }

            foreach (Weaponlaboratory cannabislab in Laboratories.WeaponlaboratoryModule.Instance.GetAll().Values.Where(m => m.ActingPlayers.Count > 0))
            {
                float Distance = iPlayer.Player.Position.DistanceTo(cannabislab.JumpPointEingang.Position);
                float maxDistance = cannabislab.ActingPlayers.Count() * MethModule.DrugLabIncreaseRange;
                if (maxDistance > 150) maxDistance = 150;
                if (Distance < maxDistance)
                {
                    int percDist = 10 - ((int)((Distance / 60) * 10));
                    int Stickstoff = Convert.ToInt32(0.78 * (100 - percDist));
                    int Sauerstoff = Convert.ToInt32(0.20 * (100 - percDist));
                    iPlayer.SendNewNotification($"Lufttest: Stickstoff {Stickstoff}%, Sauerstoff {Sauerstoff}%, Schwefel {percDist}%");
                    return true;
                }
            }

            iPlayer.SendNewNotification($"Lufttest: Stickstoff 78%, Sauerstoff 22%, Aldehyde 0%");
            return true;
        }
    }
}