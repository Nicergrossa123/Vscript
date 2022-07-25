
//using Discord;
//using Discord.Commands;
//using Discord.Rest;
//using Discord.WebSocket;
//using Nexus.Module.Chat;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;



//namespace Nexus.DiscordBot
//{
//  public class BotManager
//  {
//    public static 
  
//    DiscordSocketClient BotClient;
//    public static CommandService Commands;
//    public static IServiceProvider ServiceProvider;
//    public const string PREFIX = "!";
//    public static List<string> codelist = new List<string>();

//    public async Task RunBot()
//    {
//      BotManager.BotClient = new DiscordSocketClient();
//      await ((BaseDiscordClient) BotManager.BotClient).LoginAsync((TokenType) 1, Secret.GetToken(), true);
//      await ((BaseSocketClient) BotManager.BotClient).StartAsync();
//      ((BaseDiscordClient) BotManager.BotClient).Log += new Func<LogMessage, Task>(this.BotLoggtWas);
//      BotManager.BotClient.Ready += new Func<Task>(this.BotIstBereit);
//    }

//    public Task BotLoggtWas(LogMessage message)
//    {
//      Console.ForegroundColor = ConsoleColor.Cyan;
//      Console.WriteLine("Discord Bot Log" + message.ToString());
//      Console.ForegroundColor = ConsoleColor.White;
//      return Task.CompletedTask;
//    }

//    public async Task BotIstBereit() => ((BaseSocketClient) BotManager.BotClient).MessageReceived += new Func<SocketMessage, Task>(this.Nachricht);

//    public Task Nachricht(SocketMessage message)
//    {
//      try
//      {
//        if (!message.Content.StartsWith("$") || message.Author.IsBot || (long) ((SocketEntity<ulong>) message.Author).Id == (long) ((SocketEntity<ulong>) ((BaseSocketClient) BotManager.BotClient).CurrentUser).Id || message.Author.IsWebhook)
//          return Task.CompletedTask;
//        int num = !message.Content.Contains(' ') ? message.Content.Length : message.Content.IndexOf(' ');
//        if (message.Author is SocketGuildUser author)
//        {
//          if (((IEnumerable<SocketRole>) author.Roles).Any<SocketRole>((Func<SocketRole, bool>) (r => ((SocketEntity<ulong>) r).Id == 990505854472179712)))
//          {
//            string oldValue = message.Content.Substring(1, num - 1);
//            Console.WriteLine(oldValue);
//            Console.WriteLine((object) message);
//            if (oldValue.Equals("announce"))
//            {
//              string message1 = string.Join(" ", (object) message).Replace("!", "").Replace(oldValue, "");
//              Console.WriteLine(message1);
//              Chats.SendGlobalMessage(message1, Chats.COLOR.RED, Chats.ICON.GLOB);
//              message.Channel.SendMessageAsync("Eine Announce auf dem Gameserver mit dem Text '" + message1 + "' wurde Versendet.", false, (Embed) null, (RequestOptions) null, (AllowedMentions) null, (MessageReference) null, (MessageComponent) null, (ISticker[]) null, (Embed[]) null, (MessageFlags) 0);
//              return Task.CompletedTask;
//            }
//                        if (oldValue.Equals("restart"))
//                        {
//                            string message1 = string.Join(" ", (object)message).Replace("!", "").Replace(oldValue, "");
                         
//                            message.Channel.SendMessageAsync("Ingame Server wird Neugestartet.", false, (Embed)null, (RequestOptions)null, (AllowedMentions)null, (MessageReference)null, (MessageComponent)null, (ISticker[])null, (Embed[])null, (MessageFlags)0);
//                            Environment.Exit(1);
//                            return Task.CompletedTask;
//                        }
//                    }
//          else
//            message.Channel.SendMessageAsync("Wo sind deine Rechte? lol", false, (Embed) null, (RequestOptions) null, (AllowedMentions) null, (MessageReference) null, (MessageComponent) null, (ISticker[]) null, (Embed[]) null, (MessageFlags) 0);
//        }
//      }
//      catch (Exception ex)
//      {
//        Console.WriteLine("DiscordBotMessage" + ex?.ToString());
//      }
//      return Task.CompletedTask;
//    }
//  }
//}
