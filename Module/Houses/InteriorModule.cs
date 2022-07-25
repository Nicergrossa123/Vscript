using System.Collections.Generic;

namespace Nexus.Module.Houses
{
    public sealed class InteriorModule : SqlModule<InteriorModule, Interior, uint>
    {
        protected override string GetQuery()
        {
            return "SELECT * FROM `interiors` ORDER BY id;";
        }
    }
}