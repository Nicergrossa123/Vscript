using System;
using System.Collections.Generic;
using Nexus.Module.Events.EventMaps;
using Nexus.Module.MapParser;
using Nexus.Module.Events.EventNpc;
using System.Linq;
using Nexus.Module.NpcSpawner;

namespace Nexus.Module.Events.EventNpc
{
    public sealed class EventNpcModule : SqlModule<EventNpcModule, EventNpc, uint>
    {
        protected override string GetQuery()
        {
            return "SELECT * FROM `event_npc`;";
        }

        public override Type[] RequiredModules()
        {
            return new[]
            {
                typeof(NpcSpawnerModule),
                typeof(EventModule)
            };
        }

        protected override void OnItemLoad(EventNpc u)
        {
            if (EventModule.Instance.IsEventActive(u.EventId))
                new Npc(u.PedHash, u.Position, u.Heading, u.Dimension);
        }
    }
}
