using System;
using System.Collections.Generic;
using System.Text;
using Nexus.Module.Players.Db;

namespace Nexus.Module.Outfits
{
    public static class OutfitsPlayerExtension
    {
        public static void SetOutfit(this DbPlayer dbPlayer, OutfitTypes outfitType)
        {
            OutfitsModule.Instance.SetPlayerOutfit(dbPlayer, (int)outfitType);
        }
    }
}
