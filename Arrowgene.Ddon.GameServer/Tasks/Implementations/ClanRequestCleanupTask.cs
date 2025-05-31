using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Model.Scheduler;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Tasks.Implementations
{
    public class ClanRequestCleanupTask : WeeklyTask
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanRequestCleanupTask));

        public ClanRequestCleanupTask(DayOfWeek day, uint hour, uint minute) : base(TaskType.ClanRequestCleanup, day, hour, minute)
        {
        }

        public override bool IsEnabled(DdonGameServer server)
        {
            return true;
        }

        public override void RunTask(DdonGameServer server)
        {
            Logger.Info("Performing weekly clan request cleanup");
            server.Database.DeleteOldClanRequests();
        }
    }
}
