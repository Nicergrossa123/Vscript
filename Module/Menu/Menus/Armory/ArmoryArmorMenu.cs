﻿using GTANetworkAPI;
using System;
using System.Linq;
using Nexus.Module.Armory;
using Nexus.Module.GTAN;
using Nexus.Module.Logging;
using Nexus.Module.Menu;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Staatskasse;
using Nexus.Module.Weapons.Component;
using Nexus.Module.Weapons.Data;

namespace Nexus
{
    public class ArmoryArmorMenuBuilder : MenuBuilder
    {
        public ArmoryArmorMenuBuilder() : base(PlayerMenu.ArmoryArmorMenu)
        {
        }

        public override Menu Build(DbPlayer iPlayer)
        {
            var menu = new Menu(Menu, "Armory Rüstungen");

            menu.Add(MSG.General.Close(), "");

            if (!iPlayer.HasData("ArmoryId")) return menu;
            var ArmoryId = iPlayer.GetData("ArmoryId");
            Armory Armory = ArmoryModule.Instance.Get(ArmoryId);
            if (Armory == null) return menu;

            foreach (var ArmoryWeapon in Armory.ArmoryArmors)
            {
                menu.Add("R: " + ArmoryWeapon.RestrictedRang + " " + ArmoryWeapon.Name);
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
                if (!iPlayer.HasData("ArmoryId")) return false;
                var ArmoryId = iPlayer.GetData("ArmoryId");
                Armory Armory = ArmoryModule.Instance.Get(ArmoryId);
                if (Armory == null) return false;

                if (index == 0)
                {
                    MenuManager.DismissMenu(iPlayer.Player, (int)PlayerMenu.ArmoryWeapons);
                    return false;
                }
                else
                {
                    int actualIndex = 0;
                    foreach (ArmoryArmor armoryArmor in Armory.ArmoryArmors)
                    {
                        if (actualIndex == index - 1)
                        {
                            // Rang check
                            if (iPlayer.TeamRank < armoryArmor.RestrictedRang)
                            {
                                iPlayer.SendNewNotification(
                                    "Sie haben nicht den benötigten Rang für diese Schutzweste!");
                                return false;
                            }

                            if (!iPlayer.IsInDuty() && !iPlayer.IsNSADuty)
                            {
                                iPlayer.SendNewNotification(
                                    "Sie müssen dafür im Dienst sein!");
                                return false;
                            }

                            if(iPlayer.Player.Armor < 90)
                            {
                                iPlayer.SendNewNotification(
                                    "Sie müssen zuerst eine Schutzweste anziehen um das Aussehen zu ändern!");
                                return false;
                            }

                            iPlayer.VisibleArmorType = armoryArmor.VisibleArmorType;
                            iPlayer.SaveArmorType(armoryArmor.VisibleArmorType);
                            iPlayer.SetArmor(armoryArmor.ArmorValue, true);
                            return false;
                        }

                        actualIndex++;
                    }
                }

                return false;
            }
        }
    }
}
