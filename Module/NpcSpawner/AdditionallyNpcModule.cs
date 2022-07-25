using System;
using System.Collections.Generic;
using System.Text;
using Nexus.Module.Players;
using Nexus.Module.Spawners;

namespace Nexus.Module.NpcSpawner
{
    public class AdditionallyNpcModule : SqlModule<AdditionallyNpcModule, AdditionallyNpc, uint>
    {
        protected override string GetQuery()
        {
            return "SELECT * FROM `additionally_npcs` WHERE deactivated = 0;";
        }
    }
}
