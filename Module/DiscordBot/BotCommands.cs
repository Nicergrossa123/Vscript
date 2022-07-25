using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using Nexus.Module.Chat;
using System.Linq;

namespace nexusrp.Module.DiscordBot
{
    class BotCommands : BaseCommandModule
    {
        [DSharpPlus.CommandsNext.Attributes.Command("ping")]
        public async Task Ping(CommandContext ctx)
        {
            ctx.Channel.SendMessageAsync("Pong " + ctx.Member.Mention).ConfigureAwait(false);
        }

        [DSharpPlus.CommandsNext.Attributes.Command("players")]
        public async Task Players(CommandContext ctx)
        {
            ctx.Channel.SendMessageAsync($"Es sind aktuell {NAPI.Pools.GetAllPlayers().Count} Spieler online!").ConfigureAwait(false);
        }

        [DSharpPlus.CommandsNext.Attributes.Command("announce")]
        public async Task Announce(CommandContext ctx, string message = null)
        {
            if (!ctx.Member.Roles.Any(x => x.Id == 990505854472179712))
            {
                ctx.Channel.SendMessageAsync("Morgen hast keine Rechte").ConfigureAwait(false);
            }
            if (message == null | message == "")
            {
                ctx.Channel.SendMessageAsync("usage $announce 't e x t'.").ConfigureAwait(false);
                return;
            }
            
            Chats.SendGlobalMessage(message, Chats.COLOR.RED, Chats.ICON.GLOB);
            ctx.Channel.SendMessageAsync("Erfolgreich eine Globale Nachricht gesendet").ConfigureAwait(false);
        }

        [DSharpPlus.CommandsNext.Attributes.Command("restart")]
        public async Task Restart(CommandContext ctx)
        {
            if (!ctx.Member.Roles.Any(x => x.Id == 990505854472179712))
            {
                ctx.Channel.SendMessageAsync("Morgen hast keine Rechte").ConfigureAwait(false);
            }

            ctx.Channel.SendMessageAsync("Ingame Server wird Neugestartet.").ConfigureAwait(false);
            Environment.Exit(1);
            
        }
    }
}
