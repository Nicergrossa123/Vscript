using Nexus.Module.Players.Db;
using Nexus.Module.Support;

namespace Nexus.Module.Computer.Apps.SupportApp
{
    public sealed class SupportAppModule : Module<SupportAppModule>
    {
        public override bool Load(bool reload = false)
        {
            return true;
        }
    }
}
