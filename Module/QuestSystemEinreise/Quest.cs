using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Nexus.Module;
using Nexus.Module.Items;
using Nexus.Module.Logging;
using Nexus.Module.Menu;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;

namespace nexusrp.Module.QuestSystemEinreise
{
    public class QuestSystemEinreise : Module<QuestSystemEinreise>
    {
        public static Vector3 GeilerTypderautosundgeldgibt = new Vector3(-218.09, -1163.06, 23.03);

        public override bool Load(bool reload = false)
        {
            return true;
        }
        public override bool OnKeyPressed(DbPlayer dbPlayer, Key key)
        {
            if (dbPlayer.Player.IsInVehicle) return false;
            if (key == Key.E)
            {
                if (dbPlayer.Player.Position.DistanceTo(GeilerTypderautosundgeldgibt) < 1.5f && dbPlayer.HasData("QuestAn"))
                {
                    dbPlayer.Player.SendNotification("Hey, erstmal willkommen in Los Santos. Da du mich gefunden hast Habe ich ein Handy und 25k für dich!");
                    dbPlayer.GiveMoney(25000);
                    if (!dbPlayer.Container.AddItem(174, 1))
                    {
                        dbPlayer.SendNewNotification("Du hast keinen Platz fuer dieses Item!");
                        return false;
                    }

                    dbPlayer.Container.AddItem(174, 1);
                    dbPlayer.SetData("QuestAn", 0);
                }
            }
            return false;
        }
    }
}
