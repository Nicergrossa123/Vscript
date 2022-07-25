using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nexus.Handler;
using Nexus.Module.PlayerUI.Components;
using Nexus.Module.Menu;
using Nexus.Module.NSA.Observation;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.Windows;
using Nexus.Module.ReversePhone;
using Nexus.Module.Telefon.App;
namespace Nexus.Module.FIB.Menu
{
    public class FIBPhoneHistoryMenu : MenuBuilder
    {
        public FIBPhoneHistoryMenu() : base (PlayerMenu.FIBPhoneHistoryMenu)
        {

        }

        public override Module.Menu.Menu Build(DbPlayer p_DbPlayer)
        {
            if (!p_DbPlayer.HasData("fib_phone_history"))
                return null;

            DbPlayer l_Target = Players.Players.Instance.FindPlayer(p_DbPlayer.GetData("fib_phone_history"));
            if (l_Target == null || !l_Target.IsValid())
                return null;

            var l_Menu = new Module.Menu.Menu(Menu, "Telekommunikationsdaten");
            l_Menu.Add($"Schließen");

            if (!ReversePhoneModule.Instance.phoneHistory.ContainsKey(l_Target.Id))
                return l_Menu;

            var l_Histories = ReversePhoneModule.Instance.phoneHistory[l_Target.Id];

            foreach (var l_History in l_Histories.ToList())
            {
                l_Menu.Add($"[{l_History.Time.ToString()}] An: {l_History.Number.ToString()} ({(l_History.Dauer / 60).ToString()} min");
            }

            return l_Menu;
        }

        public override IMenuEventHandler GetEventHandler()
        {
            return new EventHandler();
        }

        private class EventHandler : IMenuEventHandler
        {
            public bool OnSelect(int p_Index, DbPlayer p_DbPlayer)
            {
                MenuManager.DismissCurrent(p_DbPlayer);
                return true;
            }
        }
    }
}
