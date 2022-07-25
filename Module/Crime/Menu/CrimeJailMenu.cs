using System;
using System.Collections.Generic;
using System.Linq;
using Nexus.Module.Assets.Tattoo;
using Nexus.Module.Business;
using Nexus.Module.PlayerUI.Components;
using Nexus.Module.GTAN;
using Nexus.Module.Houses;
using Nexus.Module.Items;
using Nexus.Module.Menu;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.Windows;
using Nexus.Module.Tattoo;

namespace Nexus.Module.Crime
{
    public class CrimeJailMenuBuilder : MenuBuilder
    {
        public CrimeJailMenuBuilder() : base(PlayerMenu.CrimeJailMenu)
        {
        }

        public override Menu.Menu Build(DbPlayer iPlayer)
        {
            if (!iPlayer.IsACop() || !iPlayer.Duty) return null;
            
            var menu = new Menu.Menu(Menu, "Gefaengnisuebersicht");

            menu.Add($"Schließen");

            foreach (DbPlayer jailPlayer in Players.Players.Instance.GetValidPlayers().Where(x => x.jailtime[0] > 0).ToList())
            {
                menu.Add($"{jailPlayer.GetName()} | {jailPlayer.jailtime[0]} Hafteinheiten");
            }
            return menu;
        }

        public override IMenuEventHandler GetEventHandler()
        {
            return new EventHandler();
        }

        private class EventHandler : IMenuEventHandler
        {
            public bool OnSelect(int index, DbPlayer iPlayer)
            {
                MenuManager.DismissCurrent(iPlayer);
                return false;
            }
        }
    }
}