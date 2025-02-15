using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Model.Scheduler;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Tasks
{
    public class RankingBoardResetTask : WeeklyTask
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(RankingBoardResetTask));

        public RankingBoardResetTask(DayOfWeek day, uint hour, uint minute) : base(TaskType.RankingBoardReset, day, hour, minute)
        {
        }

        public override bool IsEnabled(DdonGameServer server)
        {
            return true;
        }

        public override void RunTask(DdonGameServer server)
        {
            // TODO: Do something with these ranks like hand out rewards?
            Logger.Info("Performing weekly EXM ranking board");
            server.Database.DeleteAllRankRecords();
        }
    }
}
