using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nexus.Module.PlayerUI.Components;
using Nexus.Module.Configurations;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.Windows;

namespace Nexus.Module.Items.Scripts
{
    public static partial class ItemScript
    {
        public static async Task<bool> SellHouseScript(DbPlayer iPlayer)
        {
            if (iPlayer.Player.IsInVehicle) return false;

            if (iPlayer.job[0] != (int)jobs.JOB_Makler) return false;
            if (!ServerFeatures.IsActive("makler-haus"))
            {
                iPlayer.SendNewNotification("Diese Funktion ist derzeit deaktiviert. Weitere Informationen findest du im Forum.");
                return false;
            }

            await Task.Delay(1);
            NAPI.Task.Run(() => ComponentManager.Get<TextInputBoxWindow>().Show()(iPlayer, new TextInputBoxWindowObject() 
            { Title = "Haus verkaufen", Callback = "maklerHouseApplyObjectId", Message = "Nummer der Immobilie:" }));


            return false;
        }
    }
}