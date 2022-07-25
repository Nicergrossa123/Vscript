using System;
using System.Collections.Generic;
using System.Linq;
using Nexus.Module.Events.EventMaps;
using Nexus.Module.Events.EventNpc;
using Nexus.Module.Events.EventWeather;
using Nexus.Module.MapParser;
using Nexus.Module.NpcSpawner;

namespace Nexus.Module.Events
{
    public sealed class EventModule : SqlModule<EventModule, EventData, uint>
    {
        protected override string GetQuery()
        {
            return "SELECT * FROM `event_info`;";
        }

        public bool IsEventActive(uint eventId)
        {
            if (Get(eventId) != null)
                return Get(eventId).IsActive;

            return false;
        }
    }
}