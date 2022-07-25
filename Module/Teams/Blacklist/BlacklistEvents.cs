using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Nexus.Module.Injury;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;

namespace Nexus.Module.Teams.Blacklist
{
    public class BlacklistEvents : Script
    {
        [RemoteEvent]
        public void SetBlacklist(Player player, string returnstring)
        {
            var dbPlayer = player.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid()) return;
            
            DbPlayer target = Players.Players.Instance.FindPlayer(returnstring);
            if (target != null && target.IsValid() && target.isInjured() && target.Level >= 5)
            {
                dbPlayer.SetData("blsetplayer", target.Id);
                
                Module.Menu.MenuManager.Instance.Build(Nexus.Module.Menu.PlayerMenu.BlacklistTypeMenu, dbPlayer).Show(dbPlayer);
            }
            return;
        }
    }
}
