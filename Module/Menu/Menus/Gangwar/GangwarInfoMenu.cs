﻿using System;
using Nexus.Module.Chat;
using Nexus.Module.Gangwar;
using Nexus.Module.Items;
using Nexus.Module.Menu;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Teams.Shelter;

namespace Nexus
{
    public class GangwarInfoMenuBuilder : MenuBuilder
    {
        public GangwarInfoMenuBuilder() : base(PlayerMenu.GangwarInfo)
        {
        }

        public override Menu Build(DbPlayer iPlayer)
        {
            GangwarTown gangwar = GangwarTownModule.Instance.GetByPosition(iPlayer.Player.Position);
            var menu = new Menu(Menu, gangwar.Name);

            menu.Add(MSG.General.Close(), "");

            menu.Add("Information", "Informationen zum Gebiet");

            if (iPlayer.IsAGangster())
            {
                if (gangwar.OwnerTeam == null || iPlayer.Team != gangwar.OwnerTeam)
                {
                    menu.Add("Gebiet angreifen", "");
                }
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
                GangwarTown gangwar = GangwarTownModule.Instance.GetByPosition(iPlayer.Player.Position);
                if (gangwar == null) return true;

                if (index == 1)
                {
                    iPlayer.SendNewNotification($"Besitzer: {gangwar.OwnerTeam.Name} Letzter Angriff vor { Convert.ToInt32(DateTime.Now.Subtract(gangwar.LastAttacked).TotalHours)} Stunden",
                        PlayerNotification.NotificationType.INFO, $"Gebietsinformation {gangwar.Name}",
                        10000);
                }
                else if (index == 2)
                { 
                    if (iPlayer.IsAGangster())
                    {
                        gangwar.Attack(iPlayer);
                    }
                }
                return true;
            }
        }
    }
}