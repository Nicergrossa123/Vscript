﻿using Nexus.Module.Business;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Warrants;

namespace Nexus.Module.Menu.Menus.Business
{
    public class BusinessEnterMenuBuilder : MenuBuilder
    {
        public BusinessEnterMenuBuilder() : base(PlayerMenu.BusinessEnter)
        {
        }

        public override Menu Build(DbPlayer iPlayer)
        {
            var menu = new Menu(Menu, "Business Tower");

            menu.Add(MSG.General.Close(), "");

            if (iPlayer.IsMemberOfBusiness())
            {
                if (iPlayer.ActiveBusiness != null)
                {
                    menu.Add("[EIGENES] " + iPlayer.ActiveBusiness.Name, "");
                }
            }
            else
            {
                menu.Add("Kein eigenes vorhanden.");
            }

            foreach (var biz in BusinessModule.Instance.GetOpenBusinesses())
            {
                if (biz.Locked) menu.Add(biz.Name, "");
                else menu.Add(biz.Name, "");
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
                if (iPlayer.IsMemberOfBusiness() && index == 1 && iPlayer.ActiveBusiness != null)
                {
                    iPlayer.StopAnimation();
                    var biz = iPlayer.ActiveBusiness;
                    iPlayer.DimensionType[0] = DimensionType.Business;
                    iPlayer.Player.SetPosition(BusinessModule.BusinessPosition);
                    iPlayer.SetDimension(biz.Id);
                    biz.Visitors.Add(iPlayer);
                    return true;
                }

                var point = 2;
                foreach (var biz in BusinessModule.Instance.GetOpenBusinesses())
                {
                    if (index == point)
                    {
                        iPlayer.StopAnimation();
                        iPlayer.DimensionType[0] = DimensionType.Business;
                        iPlayer.Player.SetPosition(BusinessModule.BusinessPosition);
                        iPlayer.SetDimension(biz.Id);
                        biz.Visitors.Add(iPlayer);
                        return true;
                    }

                    point++;
                }
                
                return true;
            }
        }
    }
}