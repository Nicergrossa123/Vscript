using System;
using System.Runtime.CompilerServices;
using Nexus.Module.Commands;
using Nexus.Module.Injury;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;

namespace Nexus.Module.RemoteEvents
{
    public static class PlayerRemoteEventPermissions
    {
        public static bool CanAccessRemoteEvent(this DbPlayer dbPlayer, [CallerMemberName] string callerName = "")
        {
            if (!dbPlayer.IsValid()) return false;
            if (Configurations.Configuration.Instance.DevMode) return true;
            var methodName = callerName.ToLower();
            var remoteEventPermission = RemoteEventPermissions.Instance[methodName];
            if (remoteEventPermission == null) return true;
            if (remoteEventPermission.TeamId != null && dbPlayer.TeamId != remoteEventPermission.TeamId) return false;
            if (remoteEventPermission.PlayerRankPermission && !dbPlayer.Rank.CanAccessEvent(methodName))
                return false;
            if (!remoteEventPermission.AllowedDeath && dbPlayer.isInjured()) return false;
            if (!remoteEventPermission.AllowedOnCuff && dbPlayer.IsCuffed) return false;
            if (!remoteEventPermission.AllowedOnTied && dbPlayer.IsTied) return false;
            return true;
        }

        public static bool CheckForSpam(this DbPlayer dbPlayer, DbPlayer.OperationType operationType)
        {
            if (!dbPlayer.IsValid())
                return false;

            if (!dbPlayer.SpamProtection.ContainsKey(operationType))
            {
                dbPlayer.SpamProtection.TryAdd(operationType, DateTime.Now);
                return true;
            }

            int antiSpamDuration = GetAntiSpamDuration(operationType);

            if (dbPlayer.SpamProtection[operationType].AddSeconds(antiSpamDuration) > DateTime.Now)
                return false;

            dbPlayer.SpamProtection[operationType] = DateTime.Now;
            return true;
        }

        private static int GetAntiSpamDuration(DbPlayer.OperationType operationType)
        {
            int duration = 2;

            switch (operationType)
            {
                case DbPlayer.OperationType.PressedJ:
                case DbPlayer.OperationType.PressedH:
                case DbPlayer.OperationType.Smartphone:
                    duration = 1;
                    break;
                case DbPlayer.OperationType.BusinessCreate:
                    duration = 30;
                    break;
                case DbPlayer.OperationType.ClothesPacked:
                case DbPlayer.OperationType.ContactAdd:
                case DbPlayer.OperationType.ContactRemove:
                case DbPlayer.OperationType.ContactUpdate:
                    duration = 10;
                    break;
                case DbPlayer.OperationType.WeaponAmmoSync:
                case DbPlayer.OperationType.InventoryOpened:
                    duration = 3;
                    break;
            }

            return duration;
        }
    }
}