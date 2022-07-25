using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Nexus.Module.PlayerUI.Apps;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Telefon.App.Settings.Wallpaper;

namespace Nexus.Module.Telefon.App.Settings
{
    public class SettingsEditWallpaper : SimpleApp
    {
        public SettingsEditWallpaper() : base("SettingsEditWallpaperApp") { }

        [RemoteEvent]
        public void requestWallpaperList(Player player)
        {
            DbPlayer dbPlayer = player.GetPlayer();
            TriggerEvent(player, "responseWallpaperList", WallpaperModule.Instance.getJsonWallpapersForPlayer(dbPlayer));

        }

        [RemoteEvent]
        public void saveWallpaper(Player player, int wallpaperId)
        {
            DbPlayer dbPlayer = player.GetPlayer();
            dbPlayer.wallpaper = WallpaperModule.Instance.Get((uint)wallpaperId);
            dbPlayer.SaveWallpaper();
        }
        
    }

}
