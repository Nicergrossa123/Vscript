using System;
using System.Collections.Generic;
using System.Text;
using Nexus.Handler;
using Nexus.Module.Attachments;
using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Vehicles;

namespace Nexus.Module.Injury
{
    public class InjuryMedicModule : Module<InjuryMedicModule>
    {
        public override bool OnKeyPressed(DbPlayer dbPlayer, Key key)
        {
            if (key != Key.E || dbPlayer.Player.IsInVehicle || !dbPlayer.IsAMedic() || !dbPlayer.IsInDuty()) return false;

            SxVehicle sxVeh = VehicleHandler.Instance.GetClosestVehicleFromTeam(dbPlayer.Player.Position, (int)teams.TEAM_MEDIC, 3);

            if(sxVeh != null && sxVeh.IsValid())
            {
                if (!dbPlayer.Attachments.ContainsKey((int)Attachment.MEDICBAG))
                {
                    AttachmentModule.Instance.AddAttachment(dbPlayer, (int)Attachment.MEDICBAG);
                }
                else
                {
                    AttachmentModule.Instance.RemoveAttachment(dbPlayer, (int)Attachment.MEDICBAG);
                }
                return true;
            }
            return false;
        }
    }
}
