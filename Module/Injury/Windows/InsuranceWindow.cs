using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Nexus.Module.PlayerUI.Windows;
using Nexus.Module.Logging;
using Nexus.Module.Players.Db;
using Nexus.Module.Players;
using Nexus.Module.Items;
using Nexus.Module.Voice;
using Nexus.Module.Chat;
using Nexus.Module.Business.NightClubs;
using System.Threading.Tasks;
using Nexus.Module.Schwarzgeld;
using Nexus.Module.NSA;
using Nexus.Module.Teams;
using Nexus.Module.Events.CWS;
using Nexus.Module.Teams.Shelter;

namespace Nexus.Module.Injury.Windows
{
    
    public class InsuranceWindow : Window<Func<DbPlayer, bool>>
    {
        private class ShowEvent : Event
        {
            public ShowEvent(DbPlayer dbPlayer) : base(dbPlayer)
            {
            }
        }

        public InsuranceWindow() : base("Insurance")
        {
        }

        public override Func<DbPlayer, bool> Show()
        {
            return (player) => OnShow(new ShowEvent(player));
        }

        [RemoteEvent]
        public void setInsurance(Player Player, int insuranceType)
        {
            DbPlayer iPlayer = Player.GetPlayer();

            if (iPlayer == null || !iPlayer.IsValid()) return;

            if (insuranceType < 0 || insuranceType > 2) return;

            if(iPlayer.InsuranceType == insuranceType)
            {
                iPlayer.SendNewNotification("Du hast diese Art von Krankenversicherung bereits aktiv!");
                return;
            }

            switch(insuranceType)
            {
                case 0:
                    iPlayer.SendNewNotification("Du hast dich für keine Krankenversicherung entschieden, alle Kosten trägst du nun selbst!");
                    iPlayer.InsuranceType = insuranceType;
                    break;
                case 1:
                    iPlayer.SendNewNotification("Du hast dich für eine Krankenversicherung entschieden, es werden 50% der Behandlungs und Komakosten übernommen!");
                    iPlayer.InsuranceType = insuranceType;
                    break;
                case 2:
                    iPlayer.SendNewNotification("Du hast dich für eine private Krankenversicherung entschieden, es werden 100% der Behandlungs und Komakosten übernommen!");
                    iPlayer.InsuranceType = insuranceType;
                    break;
            }


            string insurance = "keine";
            if (iPlayer.InsuranceType == 1)
            {
                insurance = "vorhanden";
            }
            else if (iPlayer.InsuranceType == 2)
            {
                insurance = "privat";
            }


            iPlayer.Player.TriggerEvent("setInsurance", insurance);
            iPlayer.SaveInsurance();
        }
    }
}
