using System;
using System.Linq;
using Nexus.Handler;
using Nexus.Module.Export.Menu;
using Nexus.Module.Items;
using Nexus.Module.Menu;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;

namespace Nexus.Module.Export
{
    public class ItemExportModule : SqlModule<ItemExportModule, ItemExport, uint>
    {
        public override Type[] RequiredModules()
        {
            return new[] {typeof(ItemModelModule)};
        }

        protected override string GetQuery()
        {
            return "SELECT * FROM `item_exports`;";
        }

        protected override void OnLoaded()
        {
            MenuManager.Instance.AddBuilder(new ExportItemMenu());
        }

        public override bool OnKeyPressed(DbPlayer dbPlayer, Key key)
        {
            if (dbPlayer == null || !dbPlayer.IsValid() || key != Key.E) return false;

            ItemExportNpc itemExportNpc = ItemExportNpcModule.Instance.GetAll().Values.FirstOrDefault(ie => ie.Position.DistanceTo(dbPlayer.Player.Position) < 3.0f);

            if (itemExportNpc != null)
            {
                if(itemExportNpc.Teams.Count > 0)
                {
                    if (!itemExportNpc.Teams.Contains(dbPlayer.TeamId)) return false;
                }

                // open
                Module.Menu.MenuManager.Instance.Build(Nexus.Module.Menu.PlayerMenu.ItemExportsMenu, dbPlayer).Show(dbPlayer);
                return true;
            }
            return false;
        }
    }
}