﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using Nexus.Module.AnimationMenu;
using Nexus.Module.Logging;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Players.Phone;

namespace Nexus.Module.Configurations
{
    public class ConfigurationModule : Module<ConfigurationModule>
    {
        private readonly Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();

        public override int GetOrder()
        {
            return 3;
        }

        protected override bool OnLoad()
        {
            data.Clear();
            using (var streamReader = new StreamReader(@"configurations.xml", Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    var keyValue = line.Split('=');
                    if (keyValue.Length != 2) continue;
                    var key = keyValue[0];
                    var value = keyValue[1];
                    data[key] = value;
                }

                streamReader.Close();
            }

            Configuration.Instance = new DefaultConfiguration(data);
            return true;
        }

        public override bool OnKeyPressed(DbPlayer dbPlayer, Key key)
        {
            if (key != Key.J) return false;

            if (!dbPlayer.CanInteract()) return false;
            if (dbPlayer.Player.IsInVehicle) return false;
            if (PhoneCall.IsPlayerInCall(dbPlayer.Player)) return false;

            if (dbPlayer.HasData("salute"))
            {
                dbPlayer.ResetData("salute");
                dbPlayer.StopAnimation();
                return true;
            }
            else
            {
                dbPlayer.SetData("salute", 1);
                dbPlayer.PlayAnimation(AnimationMenuModule.Instance.animFlagDic[(uint)AnimationFlagList.WalkAndLoop], "anim@mp_player_intincarsalutestd@rds@",
                    "idle_a");
                return true;
            }
        }
    }
}