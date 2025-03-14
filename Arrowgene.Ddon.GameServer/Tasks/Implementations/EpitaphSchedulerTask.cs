using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Model.Rpc;
using Arrowgene.Ddon.Shared.Model.Scheduler;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Tasks.Implementations
{
    public class EpitaphSchedulerTask : WeeklyTask
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EpitaphSchedulerTask));

        public EpitaphSchedulerTask(DayOfWeek day, uint hour, uint minute) : base(TaskType.EpitaphRoadRewardsReset, day, hour, minute)
        {

        }

        public override bool IsEnabled(DdonGameServer server)
        {
            return server.GameSettings.GameServerSettings.EnableEpitaphWeeklyRewards;
        }

        public override void RunTask(DdonGameServer server)
        {
            Logger.Info("Performing weekly epitaph reset");
            server.Database.DeleteWeeklyEpitaphClaimedRewards();
            server.RpcManager.AnnounceAll("internal/packet", RpcInternalCommand.EpitaphRoadWeeklyReset, null);
        }
    }
}
