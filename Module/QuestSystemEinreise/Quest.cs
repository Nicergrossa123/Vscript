using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Nexus.Module;
using Nexus.Module.Menu;

namespace nexusrp.Module.QuestSystemEinreise
{
    public class QuestSystemEinreise : Module<QuestSystemEinreise>
    {
        public static Vector3 PositionPC1 = new Vector3(-1077.96, -2810.92, 27.7087);
        public static Vector3 PositionPC2 = new Vector3(-1067.82, -2811.16, 27.7087);
        public static Vector3 PositionPC3 = new Vector3(-1073.29, -2820.76, 27.7087);
        public static Vector3 PositionPC4 = new Vector3(-1083.56, -2820.45, 27.7087);

        public override bool Load(bool reload = false)
        {
            return true;
        }
    }
}
