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
    public class PaintballSpawnModule : SqlModule<PaintballSpawnModule, PaintballSpawn, uint>
    {
        protected override string GetQuery()
        {
            return "SELECT * FROM `paintball_spawns` WHERE active=1;";
        }

        public PaintballSpawn getSpawn(uint paintball_id)
        {
            var test = Instance.GetAll().Where(p => p.Value.paintball_id == paintball_id);
            return test.OrderBy(x => Guid.NewGuid()).FirstOrDefault().Value;
        }
    }

}
