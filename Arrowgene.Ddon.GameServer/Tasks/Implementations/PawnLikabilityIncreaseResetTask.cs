using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Model.Scheduler;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Tasks.Implementations
{
    public class PawnLikabilityIncreaseResetTask : DailyTask
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnLikabilityIncreaseResetTask));

        public PawnLikabilityIncreaseResetTask(uint hour, uint minute) : base(TaskType.PawnAffectionIncreaseInteractionReset, hour, minute)
        {
        }

        public override bool IsEnabled(DdonGameServer server)
        {
            return true;
        }

        public override void RunTask(DdonGameServer server)
        {
            Logger.Info("Pawn affection increase limit daily reset");
            server.Database.DeleteAllPartnerPawnLastAffectionIncreaseRecords();
        }
    }
}
