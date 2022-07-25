using System.Collections.Generic;
using GTANetworkAPI;
using Nexus.Module.PlayerUI.Apps;
using Nexus.Module.Players;
using Newtonsoft.Json;
using System;
using MySql.Data.MySqlClient;
using Nexus.Module.Chat;
using Nexus.Module.Configurations;
using Nexus.Module.Items;

using Nexus.Module.Players.Db;
using Nexus.Module.Players.Phone;
using Nexus.Module.Players.PlayerAnimations;

namespace Nexus.Module.Messenger.App
{
    public class MessengerApp : SimpleApp
    {
        public MessengerApp() : base("MessengerApp")
        {
        }

        [RemoteEvent]
        public void sendMessage(Player Player, uint number, string messageContent)
        {
        }

        [RemoteEvent]
        public void forwardMessage(Player Player, uint number, uint messageId)
        {
            // Forwars selected message in "original" and fake-proof. TBD later.
        }
    }
}