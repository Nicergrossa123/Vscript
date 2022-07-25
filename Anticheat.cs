using System.Collections.Generic;
using GTANetworkAPI;
using Nexus.Module.Logging;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Vehicles;

namespace Nexus.Anticheat
{
    public static class Anticheat
    {
        public static void ValidePlayerComponents(Player player)
        {
    //        var comps = player.(player.CurrentWeapon);

            // Defaultweapons hasent any components so....
     //       foreach (var comp in comps)
       //     {
        ///        Players.Instance.SendMessageToAuthorizedUsers("anticheat",
        //            $"ANTICHEAT (WEAPON CLIP HACK) {player.Name} :: {comp}");
        //    }
        }

        private static readonly List<WeaponHash> ForbiddenWeapons =
        new List<WeaponHash>(new[] {
            WeaponHash.Railgun, WeaponHash.Rpg, WeaponHash.Minigun, WeaponHash.Proximine, WeaponHash.Stickybomb, WeaponHash.Pipebomb
        });

        public static void CheckForbiddenWeapons(DbPlayer iPlayer)
        {
            var currW = iPlayer.Player.CurrentWeapon;

            if (!ForbiddenWeapons.Contains(currW)) return;
        }
    }
}