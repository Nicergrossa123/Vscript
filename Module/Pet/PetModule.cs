using System;
using GTANetworkAPI;
using Nexus.Module.Players.Db;
using Nexus.Module.Pet;

namespace Nexus.Module.Pet
{
    public sealed class PetModule : SqlModule<PetModule, PetData, uint>
    {
        protected override string GetQuery()
        {
            return "SELECT * FROM `pets`;";
        }

        protected override bool OnLoad()
        {
            return base.OnLoad();
        }

        public override void OnPlayerDisconnected(DbPlayer dbPlayer, string reason)
        {
            dbPlayer.RemovePet();
        }
    }
}