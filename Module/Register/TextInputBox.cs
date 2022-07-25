using GTANetworkAPI;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.Windows;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexus
{
    public static class TextInputBox
    {
        public static void OpenTextInputBox(this Player c, TextInputBoxWindowObject textInputBoxObject)
        {
            object variable = new { textBoxObject = textInputBoxObject };
            c.TriggerEvent("openWindow", new object[] { "TextInputBox", NAPI.Util.ToJson(variable) });
            c.TriggerEvent("componentReady", new object[] { "TextInputBox" });
        }
    }
}
