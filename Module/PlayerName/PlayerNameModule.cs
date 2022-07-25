using System;
using System.Collections.Generic;
using System.Text;

namespace Nexus.Module.PlayerName
{
    public class PlayerNameModule : SqlModule<PlayerNameModule, PlayerName, uint>
    {
        protected override string GetQuery()
        {
            return "SELECT * FROM `player`;";
        }
    }
}
