using System;
using System.Collections.Generic;
using System.Text;
using Nexus.Module.Configurations;

namespace Nexus.Module.Migration
{
    public class MigrationModule : Module<MigrationModule>
    {
        public override Type[] RequiredModules()
        {
            return new[] { typeof(ConfigurationModule) };
        }

        public override bool Load(bool reload = false)
        {
            return base.Load(reload);
        }
    }
}
