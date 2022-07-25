using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Nexus.Module.PlayerUI.Apps;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Voice;

namespace Nexus.Module.Telefon.App
{
    public class SettingsApp : SimpleApp
    {
        public SettingsApp() : base("SettingsApp")
        {
        }

        [RemoteEvent]
        public void requestPhoneSettings(Player player)
        {
            DbPlayer dbPlayer = player.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid()) return;
            TriggerEvent(player, "responsePhoneSettings", dbPlayer.phoneSetting.flugmodus, dbPlayer.phoneSetting.lautlos, dbPlayer.phoneSetting.blockCalls);
        }

        [RemoteEvent]
        public void savePhoneSettings(Player player, bool flugmodus, bool lautlos, bool blockCalls)
        {
            DbPlayer dbPlayer = player.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid()) return;
            dbPlayer.phoneSetting.flugmodus = flugmodus;
            dbPlayer.phoneSetting.lautlos = lautlos;
            dbPlayer.phoneSetting.blockCalls = blockCalls;

            if (flugmodus)
            {
                VoiceModule.Instance.ChangeFrequenz(dbPlayer, 0, true);
                VoiceModule.Instance.turnOffFunk(dbPlayer);
            }
        }
    }
}
