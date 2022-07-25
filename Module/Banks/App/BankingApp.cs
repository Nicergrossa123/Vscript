using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Nexus.Module.Banks.Windows;
using Nexus.Module.PlayerUI.Apps;
using Nexus.Module.Logging;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.RemoteEvents;

namespace Nexus.Module.Banks.App
{
    public class BankApp : SimpleApp
    {
        public BankApp() : base("BankApp")
        {
        }
    }

    public class BankAppOverview : SimpleApp
    {
        public BankAppOverview() : base("BankAppOverview")
        {
        }

        [RemoteEvent]
        public void requestBankAppOverview(Player player)
        {
            try
            {
                var dbPlayer = player.GetPlayer();
                if (dbPlayer == null || !dbPlayer.CanAccessRemoteEvent() || !dbPlayer.IsValid()) return;
                TriggerEvent(player, "responseBankAppOverview", dbPlayer.bank_money[0], NAPI.Util.ToJson(dbPlayer.BankHistory));
            }
            catch (Exception ex)
            {
                Logger.Crash(ex);
                return;
            }
        }
    }

    public class BankAppTransfer : SimpleApp
    {
        int bankingmaxcap = 1000000;
        int bankingmincap = 500;
        int tax = 1; //1% aktuell deaktiviert
        public BankAppTransfer() : base("BankAppTransfer")
        {
        }

        [RemoteEvent]
        public void requestBankingCap(Player player)
        {   // Achtung - BankingCap wird auch im Player abgefragt
            var dbPlayer = player.GetPlayer();
            if (dbPlayer == null || !dbPlayer.CanAccessRemoteEvent() || !dbPlayer.IsValid()) return;
            TriggerEvent(player, "responseBankingCap", bankingmaxcap, bankingmincap);
        }

        [RemoteEvent]
        public void bankingAppTransfer(Player player,String toPlayer,int amount)
        {  
            var dbPlayer = player.GetPlayer();
            if (dbPlayer == null || !dbPlayer.CanAccessRemoteEvent() || !dbPlayer.IsValid()) return;
            if (amount > bankingmaxcap) { return; }
            if (amount < bankingmincap) { return; }
            var bankwindow = new BankWindow();
            bankwindow.bankTransfer(player,amount,toPlayer);
        }

    }

}
