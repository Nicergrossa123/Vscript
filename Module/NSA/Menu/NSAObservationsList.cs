﻿using GTANetworkAPI;
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
using Nexus.Module.Telefon.App;

namespace Nexus.Module.NSA.Menu
{
    public class NSAObservationsListMenuBuilder : MenuBuilder
    {
        public NSAObservationsListMenuBuilder() : base(PlayerMenu.NSAObservationsList)
        {

        }

        public override Module.Menu.Menu Build(DbPlayer p_DbPlayer)
        {
            var l_Menu = new Module.Menu.Menu(Menu, "NSA Observationen");
            l_Menu.Add($"Schließen");
            l_Menu.Add($"Observation hinzufügen");

            foreach (NSAObservation nSAObservation in NSAObservationModule.ObservationList.Values.ToList())
            {
                DbPlayer targetOne = Players.Players.Instance.FindPlayerById(nSAObservation.PlayerId);
                if (targetOne == null || !targetOne.IsValid()) continue;

                l_Menu.Add($"{targetOne.Id} {targetOne.GetName()}");
            }
            
            return l_Menu;
        }

        public override IMenuEventHandler GetEventHandler()
        {
            return new EventHandler();
        }

        private class EventHandler : IMenuEventHandler
        {
            public bool OnSelect(int index, DbPlayer iPlayer)
            {
                if (index == 0)
                {
                    MenuManager.DismissCurrent(iPlayer);
                    return true;
                }
                else if(index == 1)
                {
                    if(!iPlayer.IsNSADuty)
                    {
                        return false;
                    }
                    ComponentManager.Get<TextInputBoxWindow>().Show()(iPlayer, new TextInputBoxWindowObject() { Title = "Obersvation hinzufügen", Callback = "AddObservationPlayer", Message = "Geben Sie einen Namen ein:" });
                    MenuManager.DismissCurrent(iPlayer);
                    return true;
                }
                else
                {
                    int idx = 2;
                    foreach (NSAObservation nSAObservation in NSAObservationModule.ObservationList.Values.ToList())
                    {
                        DbPlayer targetOne = Players.Players.Instance.FindPlayerById(nSAObservation.PlayerId);
                        if (targetOne == null || !targetOne.IsValid()) continue;

                        if (idx == index)
                        {
                            // Targetplayer Submenu...
                            iPlayer.SetData("nsa_target_player_id", targetOne.Id);
                            Module.Menu.MenuManager.Instance.Build(Nexus.Module.Menu.PlayerMenu.NSAObservationsSubMenu, iPlayer).Show(iPlayer);
                            return false;
                        }
                        idx++;
                    }
                }
                MenuManager.DismissCurrent(iPlayer);
                return true;
            }
        }
    }
}
