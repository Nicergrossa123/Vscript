using System;
using System.Collections.Generic;
using Nexus.Module.Players.Db;
using Newtonsoft.Json;
using GTANetworkAPI;
using Nexus.Module.Players;
using Nexus.Module.Business;
using Nexus.Module.PlayerUI.Windows;
using Nexus.Module.Teams.Shelter;
using Nexus.Module.GTAN;
using Nexus.Module.Tattoo;
using Nexus.Module.Gangwar;
using Nexus.Module.Players.Windows;
using Nexus.Module.Logging;
using Nexus.Module.NSA;
using Nexus.Module.Teams;
using Nexus.Module.Boerse;

namespace Nexus.Module.Banks.Windows
{
    public class BankPaymentMethod : Window<Func<DbPlayer, int, bool>>
    {
        private class ShowEvent : Event
        {

            [JsonProperty(PropertyName = "price")]
            private int Price { get; }


            public ShowEvent(DbPlayer dbPlayer, int price) :
         base(dbPlayer)
            {
                Price = price;

            }
        }

        public BankPaymentMethod() : base("PaymentMethods")
        {
        }

        public override Func<DbPlayer, int, bool> Show()
        {
            return (player, price) => OnShow(new ShowEvent(player, price));
        }
        [RemoteEvent]
        public void selectPaymentMethod(Player Player, string method)
        {
            var iPlayer = Player.GetPlayer();
            if (iPlayer == null || !iPlayer.IsValid()) return;

            iPlayer.SetData("selected", method);
            iPlayer.Player.TriggerEvent("Moneywindownocursor");

        
        }
    }
}