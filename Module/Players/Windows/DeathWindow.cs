using System;
using System.Linq;
using System.Reflection;
using Nexus.Module.Players.Db;
using GTANetworkAPI;
using Nexus.Module.Admin;
using Nexus.Module.PlayerUI.Windows;

namespace Nexus.Module.Players.Windows
{
    public class DeathWindow : Window<Func<DbPlayer, bool>>
    {
        private class ShowEvent : Event
        {
            public ShowEvent(DbPlayer dbPlayer) : base(dbPlayer)
            {
            }
        }

        public DeathWindow() : base("Death")
        {
        }

        public override Func<DbPlayer, bool> Show()
        {
            return player => OnShow(new ShowEvent(player));
        }

        public void closeDeathWindowS(Player Player)
        {
            TriggerEvent(Player, "closeDeathScreen");
        }
    }
}