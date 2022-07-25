using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using Nexus.Module.Logging;
using Nexus.Module.Players.Db;
using Nexus.Module.Spawners;
using Nexus.Module.Teams;

namespace Nexus.Module.Paintball
{
    public class PaintballAreaModule : SqlModule<PaintballAreaModule, PaintballArea, uint>
    {
        protected override string GetQuery()
        {
            return "SELECT * FROM `paintball` WHERE active=1;";
        }

        protected override void OnItemLoaded(PaintballArea pba)
        {
        }


    }

}
