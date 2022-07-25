using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Nexus.Handler;
using Nexus.Module.Assets.Hair;
using Nexus.Module.Assets.HairColor;
using Nexus.Module.Assets.Tattoo;
using Nexus.Module.Barber.Windows;
using Nexus.Module.PlayerUI.Components;
using Nexus.Module.Customization;
using Nexus.Module.GTAN;
using Nexus.Module.Logging;
using Nexus.Module.Menu;
using Nexus.Module.Players;

using Nexus.Module.Players.Db;
using Nexus.Module.Tattoo.Windows;
using Nexus.Module.Vehicles;

namespace Nexus.Module.SpawnProtection
{
    public sealed class SpawnProtectionModule : Module<SpawnProtectionModule>
    {
        public override bool Load(bool reload = false)
        {
            return true;
        }

        public override void OnPlayerFirstSpawn(DbPlayer dbPlayer)
        {
            // Set SpawnProtection
            dbPlayer.SetData("spawnProtectionSet", DateTime.Now);
            dbPlayer.SetData("ignoreGodmode", 10);
            dbPlayer.Player.TriggerEvent("setSpawnProtection", true);
        }

        public override void OnPlayerLoggedIn(DbPlayer dbPlayer)
        {
            // Set SpawnProtection
            dbPlayer.SetData("spawnProtectionSet", DateTime.Now);
            dbPlayer.SetData("ignoreGodmode", 10);
            dbPlayer.Player.TriggerEvent("setSpawnProtection", true);
        }

        public override void OnTenSecUpdate()
        {
            foreach(DbPlayer dbPlayer in Players.Players.Instance.GetValidPlayers())
            {
                if(dbPlayer.HasData("spawnProtectionSet"))
                {
                    DateTime spawnProtectionTime = dbPlayer.GetData("spawnProtectionSet");
                    if(spawnProtectionTime.AddSeconds(20) <= DateTime.Now)
                    {
                        dbPlayer.ResetData("spawnProtectionSet"); 
                        dbPlayer.SetData("ignoreGodmode", 1);
                        dbPlayer.Player.TriggerEvent("setSpawnProtection", false);
                    }
                }
            }
        }
    }
}