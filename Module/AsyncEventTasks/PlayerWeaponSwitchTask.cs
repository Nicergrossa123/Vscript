using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nexus.Module.Armory;
using Nexus.Module.Banks;
using Nexus.Module.Clothes;
using Nexus.Module.Houses;
using Nexus.Module.Logging;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Teams;
using Nexus.Module.Vehicles.Garages;
using Nexus.Module.Weapons.Component;

namespace Nexus.Module.AsyncEventTasks
{
    public static partial class AsyncEventTasks
    {
        public static void PlayerWeaponSwitchTask(Player player, WeaponHash oldgun, WeaponHash newWeapon)
        {
            DbPlayer iPlayer = player.GetPlayer();

            if (!iPlayer.IsValid()) return;

            Modules.Instance.OnPlayerWeaponSwitch(iPlayer, oldgun, newWeapon);

            if (iPlayer.IsCuffed)
            {
                iPlayer.Player.PlayAnimation("mp_arresting", iPlayer.Player.IsInVehicle ? "sit" : "idle", 0);
            }

            if (iPlayer.IsTied)
            {
                if (iPlayer.Player.IsInVehicle) iPlayer.Player.PlayAnimation("mp_arresting", "sit", 0);
                else iPlayer.Player.PlayAnimation("anim@move_m@prisoner_cuffed_rc", "aim_low_loop", 0);
            }

            if ((iPlayer.Lic_Gun[0] <= 0 && iPlayer.Level < 3) || iPlayer.hasPerso[0] == 0)
            {
                iPlayer.RemoveWeapons();
                iPlayer.ResetAllWeaponComponents();
            }

            if (iPlayer.PlayingAnimation)
            {
                NAPI.Player.SetPlayerCurrentWeapon(player, WeaponHash.Unarmed);
            }
        }
    }
}
