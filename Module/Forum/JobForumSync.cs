using Nexus.Module.Configurations;
using Nexus.Module.Players.Db;

namespace Nexus.Module.Forum
{
    public sealed class JobForumSync : SqlModule<JobForumSync, JobForumSyncItem, uint>
    {
        protected override string GetQuery()
        {
            return "SELECT * FROM `job_forumsync`;";
        }

        /*public override void OnPlayerLoggedIn(DbPlayer dbPlayer)
        {
            if (dbPlayer.ForumId == 0) return;
            if (!Configuration.Instance.DevMode)
            {
                dbPlayer.SynchronizeJobForum();
            }
        }*/
    }
}