using System;
using System.Collections.Generic;
using System.Text;
using Nexus.Module.Chat;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.RemoteEvents;

namespace Nexus.Module.Items
{
    public static class ContainerValidations
    {
        public static bool CanUseAction(this DbPlayer dbPlayer)
        {
            if (!dbPlayer.IsValid()) return false;

            // Check Cuff Die Death
            if (!dbPlayer.CanInteract()) return false;

            if (dbPlayer.HasData("disableinv") && dbPlayer.GetData("disableinv")) return false; // Show Inventory
                        
            return true;
        }
    }
}
