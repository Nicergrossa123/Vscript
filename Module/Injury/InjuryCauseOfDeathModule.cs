using System;
using Nexus.Module.Logging;

namespace Nexus.Module.Injury
{
    public class InjuryCauseOfDeathModule : SqlModule<InjuryCauseOfDeathModule, InjuryCauseOfDeath, uint>
    {
        public override Type[] RequiredModules()
        {
            return new[] { typeof(InjuryTypeModule) };
        }

        protected override string GetQuery()
        {
            return "SELECT * FROM `injury_causes_of_death`;";
        }
    }
}