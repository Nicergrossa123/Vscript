using System.Linq;
using System.Threading.Tasks;
using Nexus.Handler;
using Nexus.Module.Chat;
using Nexus.Module.Events.Jahrmarkt.Scooter;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Vehicles;

namespace Nexus.Module.Items.Scripts
{
    public static partial class ItemScript
    {
        public static async Task<bool> Scooter(DbPlayer dbPlayer, ItemModel itemModel)
        {
            if (!dbPlayer.Player.IsInVehicle) return false;

            SxVehicle sxVehicle = dbPlayer.Player.Vehicle.GetVehicle();

            if (sxVehicle == null || !sxVehicle.IsValid()) return false;

            Scooter scooter = ScooterModule.Instance.Scooters.Values.ToList().Where(s => s.sxVehicle == sxVehicle).FirstOrDefault();

            if(scooter != null)
            {
                if(scooter.CoinInserted)
                {
                    dbPlayer.SendNewNotification("Dieses Fahrzeug hat bereits einen Autoscooter Coin!");
                    return false;
                }
                else
                {
                    dbPlayer.SendNewNotification("Coin eingeschmissen, viel Spaß! Start ist ab dem nächsten Startsignal!");
                    scooter.CoinInserted = true;
                }
            }
            return true;
        }
    }
}