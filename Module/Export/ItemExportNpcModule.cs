﻿using System;
using Nexus.Handler;
using Nexus.Module.Items;

namespace Nexus.Module.Export
{
    public class ItemExportNpcModule : SqlModule<ItemExportNpcModule, ItemExportNpc, uint>
    {
        public override Type[] RequiredModules()
        {
            return new[] { typeof(ItemExportModule) };
        }

        protected override string GetQuery()
        {
            return "SELECT * FROM `item_exports_npc`;";
        }
    }
}