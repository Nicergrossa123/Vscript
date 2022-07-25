/*using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus;
using Discord.Net;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace nexusrp.Module.DiscordBot
{
    public class Bot
    {
        public string Token { get; private set; }
        public string Prefix { get; private set; }

        public Bot ()
        {
            this.Token = "OTk5NjQ5NzUyMjA1NTEyNzQ1.GB1F5J.RP0zzF6zQZXQ7f7VhmgY0vXusYbsRGIreUYCSo";
            this.Prefix = "$";
        }

        public static DiscordClient Client { get; private set; }
        public static CommandsNextExtension Commands { get; private set; }

        public async Task RunAsync()
        {
            var config = new DiscordConfiguration
            {
                Token = this.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.None
            };

            Client = new DiscordClient(config);
            Client.Ready += OnClientReady;
            

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new String[] { Prefix },
                EnableMentionPrefix = true,
                EnableDms = false
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<BotCommands>();

            await Client.ConnectAsync();
            await Task.Delay(-1);

        }

        private Task OnClientReady(DiscordClient nigga, DSharpPlus.EventArgs.ReadyEventArgs e)
        {

            return Task.CompletedTask;
        }

        public static void activityorgay()
        {
            DiscordActivity activity = new DiscordActivity();
            activity.Name = $"{GTANetworkAPI.NAPI.Pools.GetAllPlayers().Count} Spielern zu! | Nexus Roleplay";
            activity.ActivityType = ActivityType.Watching;
            Client.UpdateStatusAsync(activity);
        }


    }
}
*/