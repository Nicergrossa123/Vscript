﻿using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nexus.Handler;
using Nexus.Module.Chat;
using Nexus.Module.PlayerUI.Components;
using Nexus.Module.Logging;
using Nexus.Module.Menu;
using Nexus.Module.NSA.Observation;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.Windows;
using Nexus.Module.Telefon.App;

namespace Nexus.Module.NSA.Menu
{
    public class NSAObservationsSubMenuMenuBuilder : MenuBuilder
    {
        public NSAObservationsSubMenuMenuBuilder() : base(PlayerMenu.NSAObservationsSubMenu)
        {

        }

        public override Module.Menu.Menu Build(DbPlayer p_DbPlayer)
        {
            if (!p_DbPlayer.HasData("nsa_target_player_id")) return null;

            DbPlayer targetOne = Players.Players.Instance.FindPlayerById(p_DbPlayer.GetData("nsa_target_player_id"));
            if (targetOne == null || !targetOne.IsValid()) return null;

            NSAObservation nSAObservation = NSAObservationModule.ObservationList.ToList().FirstOrDefault(o => o.Value.PlayerId == targetOne.Id).Value;
            if (nSAObservation == null) return null;

            var l_Menu = new Module.Menu.Menu(Menu, "NSA Observation (" + targetOne.GetName() + ")");
            l_Menu.Add($"Schließen");


            if (!nSAObservation.Agreed)
            {
                if (p_DbPlayer.IsNSADuty && p_DbPlayer.IsNSAState == (int)NSARangs.LEAD)
                {
                    l_Menu.Add($"Observation genehmigen");
                }
                else
                {
                    l_Menu.Add($"Genehmigung ausstehend!");
                }
            }
            else
            {

                l_Menu.Add($"Observation beenden");
                l_Menu.Add($"Ortung starten/beenden");
                l_Menu.Add($"Banktransaktionen");
                l_Menu.Add($"Fahrzeug Schlüssel");
                l_Menu.Add("Gespräch mithören/beenden");

                if(p_DbPlayer.IsNSAState >= (int)NSA.NSARangs.NORMAL)
                {
                    l_Menu.Add($"Handy clonen (SMS)");
                }
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
                if (!iPlayer.HasData("nsa_target_player_id")) return false;

                DbPlayer targetOne = Players.Players.Instance.FindPlayerById(iPlayer.GetData("nsa_target_player_id"));
                if (targetOne == null || !targetOne.IsValid()) return false;

                NSAObservation nSAObservation = NSAObservationModule.ObservationList.ToList().FirstOrDefault(o => o.Value.PlayerId == targetOne.Id).Value;
                if (nSAObservation == null) return false;

                if (!iPlayer.IsNSADuty) return false;

                switch (index)
                {
                    case 0:
                        MenuManager.DismissCurrent(iPlayer);
                        return true;
                    case 1:
                        if (!nSAObservation.Agreed)
                        {
                            if (iPlayer.IsNSAState == (int)NSARangs.LEAD)
                            {
                                NSAObservationModule.Instance.AgreeObservation(iPlayer, nSAObservation);
                            }
                            return true;
                        }
                        else
                        {
                            NSAObservationModule.Instance.RemoveObservation(iPlayer, iPlayer.GetData("nsa_target_player_id"));
                            iPlayer.SendNewNotification("Observation beendet!");
                            return true;
                        }
                    case 2:
                        
                        if(iPlayer.HasData("nsaOrtung"))
                        {
                            iPlayer.ResetData("nsaOrtung");
                            iPlayer.SendNewNotification("Ortung beendet!");
                            return true;
                        }

                        iPlayer.SetData("nsaOrtung", targetOne.Id);
                        iPlayer.SendNewNotification($"Ortung von {targetOne.GetName()} gestartet!");

                        iPlayer.Player.TriggerEvent("setPlayerGpsMarker", targetOne.Player.Position.X, targetOne.Player.Position.Y);
                        Logger.AddFindLog(iPlayer.Id, targetOne.Id);
                        return true;
                    case 3:
                        Module.Menu.MenuManager.Instance.Build(Nexus.Module.Menu.PlayerMenu.NSABankMenu, iPlayer).Show(iPlayer);
                        return false;
                    case 4:
                        Module.Menu.MenuManager.Instance.Build(Nexus.Module.Menu.PlayerMenu.NSAVehicleObersvationListMenu, iPlayer).Show(iPlayer);
                        return false;
                    case 5:
                        if (iPlayer.HasData("nsa_activePhone"))
                        {
                            iPlayer.Player.TriggerEvent("setCallingPlayer", "");
                            iPlayer.ResetData("nsa_activePhone");
                            iPlayer.SendNewNotification("Mithören beendet!");
                            return true;
                        }
                        else
                        {
                            // Enable this if list with obersvations is active
                            if (!targetOne.HasData("current_caller")) return false;
                            if (targetOne.IsInAdminDuty()) return false;

                            DbPlayer ConPlayer = TelefonInputApp.GetPlayerByPhoneNumber(targetOne.GetData("current_caller"));
                            if (ConPlayer == null || !ConPlayer.IsValid()) return false;
                            if (ConPlayer.IsInAdminDuty()) return false;

                            if (NSAObservationModule.ObservationList.Where(o => o.Value.PlayerId == ConPlayer.Id || o.Value.PlayerId == targetOne.Id).Count() == 0 && !targetOne.IsACop() && !ConPlayer.IsACop()
                                && !targetOne.IsAMedic() && !ConPlayer.IsAMedic())
                            {
                                iPlayer.SendNewNotification("Spieler ist nicht fuer eine Observation freigegeben!");
                                return false;
                            }


                            string voiceHashPush = targetOne.VoiceHash + "~3~0~0~2;" + ConPlayer.VoiceHash;
                            iPlayer.Player.TriggerEvent("setCallingPlayer", voiceHashPush);

                            iPlayer.SetData("nsa_activePhone", targetOne.handy[0]);

                            iPlayer.SendNewNotification("Mithören gestartet " + targetOne.handy[0]);
                            NSAModule.Instance.SendMessageToNSALead($"{iPlayer.GetName()} hört nun das Telefonat von {targetOne.GetName()} mit.");
                            return false;
                        }

                    case 6:
                        if (iPlayer.IsNSAState <= (int)NSARangs.LIGHT) return false;

                        if (targetOne == null || !targetOne.IsValid() || targetOne.Id == iPlayer.Id) return false;

                        if (targetOne.phoneSetting.flugmodus)
                        {
                            iPlayer.SendNewNotification("Smartphone konnte nicht gecloned werden!");
                            return false;
                        }

                        iPlayer.SendNewNotification($"Smartphone von {targetOne.GetName()} wird gecloned!");

                        Main.m_AsyncThread.AddToAsyncThread(new Task(async () =>
                        {

                            Chats.sendProgressBar(iPlayer, (15 * 1000));
                            await Task.Delay(15 * 1000);

                            if (targetOne == null || !targetOne.IsValid()) return;
                            iPlayer.SetData("nsa_smclone", targetOne.Id);
                            iPlayer.SendNewNotification($"Smartphone von {targetOne.GetName()} wurde gecloned!");

                            NSAModule.Instance.SendMessageToNSALead($"{iPlayer.GetName()} hat das Handy von {targetOne.GetName()} gecloned.");
                        }));
                        return false;
                    default:
                        MenuManager.DismissCurrent(iPlayer);
                        return true;
                }
            }
        }
    }
}
