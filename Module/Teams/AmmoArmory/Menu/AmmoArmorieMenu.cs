using System;
using System.Collections.Generic;
using System.Linq;
using Nexus.Module.Assets.Tattoo;
using Nexus.Module.Business;
using Nexus.Module.PlayerUI.Components;
using Nexus.Module.GTAN;
using Nexus.Module.Houses;
using Nexus.Module.Items;
using Nexus.Module.Menu;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.Windows;
using Nexus.Module.Tattoo;
using Nexus.Module.Teams.AmmoPackageOrder;
using Nexus.Module.Teams.Shelter;

namespace Nexus.Module.Teams.AmmoArmory
{
    public class AmmoArmorieMenuBuilder : MenuBuilder
    {
        public AmmoArmorieMenuBuilder() : base(PlayerMenu.AmmoArmorieMenu)
        {
        }

        public override Menu.Menu Build(DbPlayer iPlayer)
        {
            AmmoArmorie ammoArmorie = AmmoArmoryModule.Instance.GetByPosition(iPlayer.Player.Position);
            if (ammoArmorie == null || !iPlayer.Team.IsGangsters() || iPlayer.Team.Id != ammoArmorie.TeamId) return null;

            var menu = new Menu.Menu(Menu, "Munitionskammer (" + ammoArmorie.Packets + "P | " + ammoArmorie.Powder +" SP)");

            menu.Add($"Schließen");

            menu.Add("Pakete abgeben");
            menu.Add("Preise einstellen");

            foreach (AmmoArmorieItem ammoArmorieItem in ammoArmorie.ArmorieItems)
            {
                menu.Add(ItemModelModule.Instance.Get(ammoArmorieItem.ItemId).Name + " $" + ammoArmorieItem.TeamPrice, "(P:" + ammoArmorieItem.GetRequiredPacketsForTeam(iPlayer.Team) + ")");
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
                if (index == 0)
                {
                    MenuManager.DismissCurrent(iPlayer);
                    return false;
                }
                else if (index == 1) // Packete abgeben
                {
                    AmmoArmorie ammoArmorie = AmmoArmoryModule.Instance.GetByPosition(iPlayer.Player.Position);
                    if (ammoArmorie == null || !iPlayer.Team.IsGangsters() || iPlayer.Team.Id != ammoArmorie.TeamId) return false;

                    int hasChests = iPlayer.Container.GetItemAmount(AmmoArmoryModule.AmmoChestItem);
                    int hasPowder = iPlayer.Container.GetItemAmount(AmmoArmoryModule.BlackPowderItem);
                    if (hasChests > 0)
                    {
                        if(ammoArmorie.Packets+(hasChests * AmmoPackageOrderModule.AmmoChestToPackageMultipliert) >= AmmoArmoryModule.MaxLagerBestand)
                        {
                            iPlayer.SendNewNotification("Maximale Anzahl erreicht! (" + AmmoArmoryModule.MaxLagerBestand + ")");
                            return false;
                        }
                        else
                        {
                            ammoArmorie.ChangePackets(hasChests * AmmoPackageOrderModule.AmmoChestToPackageMultipliert);
                            iPlayer.Container.RemoveItem(AmmoArmoryModule.AmmoChestItem, hasChests);
                            iPlayer.SendNewNotification($"Sie haben {hasChests} Kisten ({hasChests* AmmoPackageOrderModule.AmmoChestToPackageMultipliert} Pakete) eingelagert!");
                            return true;
                        }
                    }

                    if (hasPowder > 0)
                    {
                        if (ammoArmorie.Powder + (hasPowder * AmmoPackageOrderModule.BlackPowderToPackageMultiplier) >= AmmoArmoryModule.MaxLagerBestand)
                        {
                            iPlayer.SendNewNotification("Maximale Anzahl erreicht! (" + AmmoArmoryModule.MaxLagerBestand + ")");
                            return false;
                        }
                        else
                        {
                            ammoArmorie.ChangePowder(hasPowder * AmmoPackageOrderModule.BlackPowderToPackageMultiplier);
                            iPlayer.Container.RemoveItem(AmmoArmoryModule.BlackPowderItem, hasPowder);
                            iPlayer.SendNewNotification($"Sie haben {hasPowder} Schwarzpulver ({hasPowder * AmmoPackageOrderModule.BlackPowderToPackageMultiplier}) eingelagert!");
                            return true;
                        }
                    }

                    MenuManager.DismissCurrent(iPlayer);
                    return false;
                }
                else if (index == 2) // Preis einstellen
                {
                    AmmoArmorie ammoArmorie = AmmoArmoryModule.Instance.GetByPosition(iPlayer.Player.Position);
                    if (ammoArmorie == null || !iPlayer.Team.IsGangsters() || iPlayer.Team.Id != ammoArmorie.TeamId) return false;

                    if (iPlayer.TeamRank < 11) return false;
                    
                    MenuManager.Instance.Build(PlayerMenu.AmmoArmoriePriceMenu, iPlayer).Show(iPlayer);
                    return false;
                }
                else // choose x.x
                {
                    AmmoArmorie ammoArmorie = AmmoArmoryModule.Instance.GetByPosition(iPlayer.Player.Position);
                    if (ammoArmorie == null || !iPlayer.Team.IsGangsters() || iPlayer.Team.Id != ammoArmorie.TeamId) return false;

                    int idx = 3;
                    foreach (AmmoArmorieItem ammoArmorieItem in ammoArmorie.ArmorieItems)
                    {
                        if (idx == index)
                        {
                            int RequiredPackets = ammoArmorieItem.GetRequiredPacketsForTeam(iPlayer.Team);

                            if(ammoArmorie.Packets < RequiredPackets)
                            {
                                iPlayer.SendNewNotification("Nicht genug Waffenpakete!");
                                return false;
                            }

                            if (ammoArmorie.Powder < RequiredPackets)
                            {
                                iPlayer.SendNewNotification("Nicht genug Schwarzpulver!");
                                return false;
                            }

                            ItemModel resultItem = ItemModelModule.Instance.Get(ammoArmorieItem.ItemId);
                            if (resultItem == null) return false;

                            if(iPlayer.Container.CanInventoryItemAdded(resultItem))
                            {
                                if (ammoArmorieItem.TeamPrice > 0)
                                {
                                    if (!iPlayer.TakeBankMoney(ammoArmorieItem.TeamPrice))
                                    {
                                        iPlayer.SendNewNotification("Nicht genug Geld (Bank)!");
                                        return false;
                                    }

                                    TeamShelter teamShelter = TeamShelterModule.Instance.GetByTeam(iPlayer.Team.Id);
                                    if (teamShelter == null) return false;

                                    teamShelter.GiveMoney(ammoArmorieItem.TeamPrice);
                                }

                                ammoArmorie.ChangePackets(-RequiredPackets);
                                ammoArmorie.ChangePowder(-RequiredPackets);
                                iPlayer.Container.AddItem(resultItem);
                                iPlayer.SendNewNotification($"Sie haben {resultItem.Name} für ${ammoArmorieItem.TeamPrice} (P: {RequiredPackets} | SP: {RequiredPackets}) entnommen!");
                                
                                return false;
                            }
                            else
                            {
                                iPlayer.SendNewNotification("Zu wenig Platz!");
                                return false;
                            }
                        }
                        else idx++;
                    }
                    return true;
                }
            }
        }
    }
}