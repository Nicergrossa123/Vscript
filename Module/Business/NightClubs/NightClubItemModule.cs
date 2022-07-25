using System;
using Nexus.Module.Items;

namespace Nexus.Module.Business.NightClubs
{
    class NightClubItemModule : SqlModule<NightClubItemModule, NightClubItem, uint>
    {
        public override Type[] RequiredModules()
        {
            return new[] { typeof(ItemModelModule) };
        }

        protected override string GetQuery()
        {
            return "SELECT * FROM `business_nightclubs_items`;";
        }
    }
}
