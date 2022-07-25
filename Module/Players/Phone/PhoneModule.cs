using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Nexus.Handler;
using Nexus.Module.Chat;
using Nexus.Module.Injury;
using Nexus.Module.Items;
using Nexus.Module.Logging;

using Nexus.Module.Players.Db;

using Nexus.Module.Players.Phone.Apps;
using Nexus.Module.Players.Phone.Contacts;
using Nexus.Module.Players.PlayerAnimations;
using Nexus.Module.Tasks;

namespace Nexus.Module.Players.Phone
{
    public class PhoneModule : Module<PhoneModule>
    {
        public static Dictionary<int, int> ActivePhoneCalls = new Dictionary<int, int>();

        protected override bool OnLoad()
        {
            ActivePhoneCalls = new Dictionary<int, int>();
            return base.OnLoad();
        }

        public override void OnPlayerConnected(DbPlayer dbPlayer)
        {
            dbPlayer.PhoneContacts = new PhoneContacts(dbPlayer.Id);
            dbPlayer.PhoneContacts.Populate();

            dbPlayer.PhoneApps = new PhoneApps(dbPlayer);
        }
        
    }
}