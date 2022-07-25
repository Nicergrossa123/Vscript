using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nexus.Module.Players.Db;

namespace Nexus.Module.Players.Buffs
{
    public class CustomDrugModule : Module<CustomDrugModule>
    {
        public string NewDrugEffect = "DrugsMichaelAliensFight";

        public void SetCustomDrugEffect(DbPlayer dbPlayer)
        {
            dbPlayer.Player.TriggerEvent("startScreenEffect", NewDrugEffect, 60000, true);
        }
        
        public void RemoveEffect(DbPlayer dbPlayer)
        {
            dbPlayer.Player.TriggerEvent("stopScreenEffect", NewDrugEffect);
        }
        
    }
}
