using System;
using Nexus.Module.Barber;

namespace Nexus.Module.Assets.HairColor
{
    public class AssetsHairColorModule : SqlModule<AssetsHairColorModule, AssetsHairColor, uint>
    {
        protected override string GetQuery()
        {
            return "SELECT * FROM `assets_hair_color`;";
        }
    }
}
