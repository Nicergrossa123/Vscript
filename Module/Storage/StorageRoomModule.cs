using System;
using System.Linq;
using Nexus.Module.Items;
using Nexus.Module.Logging;
using Nexus.Module.Players.Db;

namespace Nexus.Module.Storage
{
    public class StorageRoomModule : SqlModule<StorageRoomModule, StorageRoom, uint>
    {
        public override Type[] RequiredModules()
        {
            return new[] { typeof(ItemModelModule) };
        }

        protected override string GetQuery()
        {
            return "SELECT * FROM `storage_rooms`;";
        }

        protected override void OnItemLoaded(StorageRoom storageRoom)
        {
            storageRoom.Container = ContainerManager.LoadContainer(storageRoom.Id, ContainerTypes.STORAGE, 0, 0);
        }

        public StorageRoom GetClosest(DbPlayer dbPlayer)
        {
            return StorageRoomModule.Instance.GetAll().FirstOrDefault(st => st.Value.Position.DistanceTo(dbPlayer.Player.Position) < 2.0f).Value;
        }
    }
}