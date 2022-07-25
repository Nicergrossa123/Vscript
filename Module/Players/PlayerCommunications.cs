using Nexus.Module.Injury;
using Nexus.Module.Players.Db;

namespace Nexus.Module.Players
{
    public static class PlayerCommunications
    {
        public static bool CanCommunicate(this DbPlayer iPlayer)
        {
            return !(iPlayer.isInjured() || iPlayer.jailtime[0] > 0 ||
                     iPlayer.IsCuffed || iPlayer.IsTied);
        }

        public static void BlockCommunications(this DbPlayer iPlayer)
        {
        }
    }
}