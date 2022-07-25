using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using Nexus.Module.Events.EventMaps;
using Nexus.Module.Events.EventNpc;
using Nexus.Module.MapParser;
using Nexus.Module.NpcSpawner;
using Nexus.Module.Weather;

namespace Nexus.Module.Events.EventWeather
{
    public sealed class EventWeatherModule : SqlModule<EventWeatherModule, EventWeather, uint>
    {
        protected override string GetQuery()
        {
            return "SELECT * FROM `event_weather`;";
        }

        public override Type[] RequiredModules()
        {
            return new[]
            {
                typeof(WeatherModule),
                typeof(EventModule)
            };
        }

        protected override void OnItemLoad(EventWeather u)
        {
            if (EventModule.Instance.IsEventActive(u.EventId))
            {
                WeatherModule.Instance.ChangeWeather((GTANetworkAPI.Weather)u.WeatherId);
            }
        }
    }
}
