using System;
using System.Linq;
using Nexus.Module.Items;
using Nexus.Module.Logging;
using Nexus.Module.Players.Db;

namespace Nexus.Module.Storage
{
    public class StorageRoomAusbaustufenModule : SqlModule<StorageRoomAusbaustufenModule, StorageRoomAusbaustufe, uint>
    {
        protected override string GetQuery()
        {
            return "SELECT * FROM `storage_rooms_ausbaustufen`;";
        }
    }
}