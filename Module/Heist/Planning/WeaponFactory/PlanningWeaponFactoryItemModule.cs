using System;
using System.Collections.Generic;
using System.Text;

namespace Nexus.Module.Heist.Planning.WeaponFactory
{
    public class PlanningWeaponFactoryItemModule : SqlModule<PlanningWeaponFactoryItemModule, PlanningWeaponFactoryItem, uint>
    {
        protected override string GetQuery()
        {
            return "SELECT * FROM `planningroom_weaponfactory_items`;";
        }
    }
}
