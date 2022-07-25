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
using Nexus.Module.Telefon.App;
using Nexus.Module.Vehicles;

namespace Nexus.Module.NSA.Menu
{
    public class NSAPeilsenderMenuBuilder : MenuBuilder
    {
        public NSAPeilsenderMenuBuilder() : base(PlayerMenu.NSAPeilsenderMenu)
        {

        }

        public override Module.Menu.Menu Build(DbPlayer p_DbPlayer)
        {
            var l_Menu = new Module.Menu.Menu(Menu, "NSA Aktive Peilsender");
            l_Menu.Add($"Schließen");

            foreach (NSAPeilsender nSAPeilsender in NSAObservationModule.NSAPeilsenders)
            {
                l_Menu.Add($"{nSAPeilsender.Name}");
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
                int i = 1;

                foreach (NSAPeilsender nSAPeilsender in NSAObservationModule.NSAPeilsenders)
                {
                    if(i == index)
                    {
                        if(nSAPeilsender.VehicleId != 0)
                        {
                            SxVehicle sxVeh = VehicleHandler.Instance.GetByVehicleDatabaseId(nSAPeilsender.VehicleId);
                            if (sxVeh == null || !sxVeh.IsValid()) return true;

                            if (iPlayer.HasData("nsaOrtung"))
                            {
                                iPlayer.ResetData("nsaOrtung");
                            }

                            iPlayer.SetData("nsaPeilsenderOrtung", (uint)nSAPeilsender.VehicleId);

                            // Orten
                            iPlayer.SendNewNotification("Peilsender geortet!");
                            return true;
                        }
                        
                        return true;
                    }
                    i++;
                }

                MenuManager.DismissCurrent(iPlayer);
                return true;
            }
        }
    }
}
