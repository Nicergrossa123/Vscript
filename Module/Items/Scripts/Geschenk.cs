using System;
using System.Threading.Tasks;
using Nexus.Module.Geschenk;
using Nexus.Module.Players.Db;

namespace Nexus.Module.Items.Scripts
{
    public static partial class ItemScript
    {
        public static bool Geschenk(DbPlayer dbPlayer)
        {
            GeschenkModule.Instance.GenerateRandomReward(dbPlayer);
            return false;
        }
    }
}
