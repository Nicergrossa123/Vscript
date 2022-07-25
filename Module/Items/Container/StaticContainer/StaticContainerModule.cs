using System;
using Nexus.Module.Logging;

namespace Nexus.Module.Items
{
    public class StaticContainerModule : SqlModule<StaticContainerModule, StaticContainer, uint>
    {
        public override Type[] RequiredModules()
        {
            return new[] { typeof(ItemsModule), typeof(ItemModelModule) };
        }

        protected override string GetQuery()
        {
            return "SELECT * FROM `container_static_data`;";
        }
    }
}