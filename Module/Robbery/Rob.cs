using System;
using System.Collections.Generic;
using System.Linq;
using Nexus.Module.Chat;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Shops;
using Nexus.Module.Teams;

namespace Nexus.Module.Robbery
{
    public class Rob
    {
        public int Id { get; set; }
        public DbPlayer Player { get; set; }
        public int Interval { get; set; }
        public int CopInterval { get; set; }
        public int EndInterval { get; set; }
        public bool Disabled { get; set; }
        public RobType Type { get; set; }
    }

    public enum RobType
    {
        Shop,
        Juwelier,
        Staatsbank,
        VespucciBank
    }
}