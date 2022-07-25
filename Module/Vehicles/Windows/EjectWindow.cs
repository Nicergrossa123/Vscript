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
using Nexus.Handler;
using Nexus.Module.Commands;

namespace Nexus.Module.Vehicles.Windows
{
    
    public class SeatPlayerItem
    {
        [JsonProperty(PropertyName = "seatid")] public uint SeatId { get; set; }
        [JsonProperty(PropertyName = "used")] public bool Used { get; set; }
    }

    public class EjectWindow : Window<Func<DbPlayer, SxVehicle, bool>>
    {
        private class ShowEvent : Event
        {
            [JsonProperty(PropertyName = "seats")] private List<SeatPlayerItem> seats { get; }
            public ShowEvent(DbPlayer dbPlayer, SxVehicle sxVehicle) : base(dbPlayer)
            {
                List<SeatPlayerItem> seatsList = new List<SeatPlayerItem>();

                for (int i = -1; i < sxVehicle.Data.Slots-1; i++)
                {
                    bool used = false;

                    Dictionary<int, DbPlayer> occu = sxVehicle.GetOccupants();
                    if(occu.ContainsKey(i))
                    {
                        if(occu[i] != null && occu[i].IsValid() && !occu[i].IsInAdminDuty())
                        {
                            used = true;
                        }
                    }

                    seatsList.Add(new SeatPlayerItem() { SeatId = (uint)i, Used = used });
                }

                seats = seatsList;
            }
        }

        public EjectWindow() : base("EjectWindow")
        {
        }

        public override Func<DbPlayer, SxVehicle, bool> Show()
        {
            return (player, veh) => OnShow(new ShowEvent(player, veh));
        }

        [RemoteEvent]
        public void ejectSeat(Player player, int seatId)
        {

            Main.m_AsyncThread.AddToAsyncThread(new Task(() =>
            {
                var dbPlayer = player.GetPlayer();
                if (!dbPlayer.CanAccessMethod()) return;

                if (dbPlayer.Player.VehicleSeat != 0)
                {
                    dbPlayer.SendNewNotification(
                        "Sie muessen Fahrer des Fahrzeuges sein!");
                    return;
                }

                if (seatId == 0) return;

                var sxVeh = dbPlayer.Player.Vehicle.GetVehicle();
                if (sxVeh == null || !sxVeh.IsValid() || sxVeh.GetOccupants() == null || sxVeh.GetOccupants().Count <= 0) return;

                if (!sxVeh.GetOccupants().TryGetValue(seatId, out DbPlayer findPlayer))
                {
                    dbPlayer.SendNewNotification(
                        "Auf diesem Platz sitzt keiner.");
                    return;
                }

                if (findPlayer == null || !findPlayer.IsValid()
                    || !findPlayer.Player.IsInVehicle
                    || findPlayer.Player.Vehicle != dbPlayer.Player.Vehicle)
                {
                    return;
                }


                if (sxVeh != null && sxVeh.GetOccupants().ContainsValue(findPlayer))
                {
                    sxVeh.Occupants.Remove(sxVeh.Occupants.First(x => x.Value == findPlayer).Key);
                }


                findPlayer.StopAnimation();
                if (findPlayer.IsCuffed)
                {
                    NAPI.Task.Run(() =>
                    {
                        findPlayer.SetCuffed(false);
                        findPlayer.WarpOutOfVehicle();
                        findPlayer.Player.SetPosition(new Vector3(dbPlayer.Player.Vehicle.Position.X + 1,
                            dbPlayer.Player.Vehicle.Position.Y + 1, dbPlayer.Player.Vehicle.Position.Z));
                        Task.Delay(2500);
                        findPlayer.SetCuffed(true);
                    });
                }
                else if (findPlayer.IsTied)
                {
                    NAPI.Task.Run(() =>
                    {
                        findPlayer.SetTied(false);
                        findPlayer.WarpOutOfVehicle();
                        findPlayer.Player.SetPosition(new Vector3(dbPlayer.Player.Vehicle.Position.X + 1,
                            dbPlayer.Player.Vehicle.Position.Y + 1, dbPlayer.Player.Vehicle.Position.Z));
                        Task.Delay(2500);
                        findPlayer.SetTied(true);
                    });
                }
                else
                {
                    findPlayer.WarpOutOfVehicle();
                }


                dbPlayer.SendNewNotification("Sie haben jemanden aus dem Fahrzeug geschmissen!");
                findPlayer.SendNewNotification("Jemand hat Sie aus dem Fahrzeug geschmissen!");
            }));

        }
    }
}
