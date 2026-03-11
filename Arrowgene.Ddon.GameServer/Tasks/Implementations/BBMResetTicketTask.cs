using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Model.Scheduler;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Tasks.Implementations
{
    public class BBMResetTicketTask(DayOfWeek day, uint hour, uint minute) : WeeklyTask(TaskType.AwardBitterblackMazeResetTickets, day, hour, minute)
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(RankingBoardResetTask));

        public override bool IsEnabled(DdonGameServer server)
        {
            return true;
        }

        public override void RunTask(DdonGameServer server)
        {
            Logger.Info("Performing BBM reset ticket handout.");

            server.Database.ExecuteInTransaction(connection =>
            {
                server.Database.ResetBBMResetTicketStatus(connection);
                server.Database.ResetBBMGGReset(connection);
            });
        }
    }
}
