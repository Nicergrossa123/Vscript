﻿using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Nexus.Module.Configurations;
using Nexus.Module.Logging;
using System.Net.Sockets;
using System.Net.Http;

namespace Nexus.Handler
{
    public class DiscordHandler
    {
        private static string m_LiveWebhookURL = "https://discord.com/api/webhooks/999667290142425189/zqj6-yLx3_v1ZPNys9i4N3Sfp6gVGfXtgoC4gQI_DnGpaOaASNSytbpW-nayqThlSmw8";
        private static string m_TestWebhookURL = "https://discord.com/api/webhooks/999667290142425189/zqj6-yLx3_v1ZPNys9i4N3Sfp6gVGfXtgoC4gQI_DnGpaOaASNSytbpW-nayqThlSmw8";
        private static string m_status = "https://discord.com/api/webhooks/999667290142425189/zqj6-yLx3_v1ZPNys9i4N3Sfp6gVGfXtgoC4gQI_DnGpaOaASNSytbpW-nayqThlSmw8";

        public DiscordHandler()
        {
        }

        // Get the IP From The Server
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Kein Netzwerkadapter mit einer IPv4-Adresse im System!");
        }

        public static void SendMessage(string p_Message, string p_Description = "")
        {
            try
            {

                DiscordMessage l_Message = new DiscordMessage($"Name: { NAPI.Server.GetServerName()} - IP: {GetLocalIPAddress()} - Port: {NAPI.Server.GetServerPort()} - {p_Message}", p_Description);

                using (WebClient l_WC = new WebClient())
                {
                    l_WC.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    l_WC.Encoding = Encoding.UTF8;

                    string l_Upload = JsonConvert.SerializeObject(l_Message);
                    if (Configuration.Instance.DevMode)
                        l_WC.UploadString(m_TestWebhookURL, l_Upload);
                    else
                        l_WC.UploadString(m_LiveWebhookURL, l_Upload);
                }
            }
            catch (Exception e)
            {
                Logger.Crash(e);
                // Muss funktionieren amk, coded by Euka
            }
        }
    }

    public class DiscordMessage
    {
        public string content { get; private set; }
        public bool tts { get; private set; }
        public EmbedObject[] embeds { get; private set; }

        public DiscordMessage(string p_Message, string p_EmbedContent)
        {
            content = p_Message;
            tts = true;

            EmbedObject l_Embed = new EmbedObject(p_EmbedContent);
            embeds = new EmbedObject[] { l_Embed };
        }
    }

    public class EmbedObject
    {
        public string title { get; private set; }
        public string description { get; private set; }

        public EmbedObject(string p_Desc)
        {
            title = DateTime.Now.ToString();
            description = p_Desc;
        }
    }
}
