using System;
using Nexus.Module.Logging;
using Nexus.Module.Spawners;

namespace Nexus.Module.Injury
{
    public class InjuryDeliverIntPointModule : SqlModule<InjuryDeliverIntPointModule, InjuryDeliverIntPoint, uint>
    {
        protected override string GetQuery()
        {
            return "SELECT * FROM `injury_deliver_int_points`;";
        }
    }
}