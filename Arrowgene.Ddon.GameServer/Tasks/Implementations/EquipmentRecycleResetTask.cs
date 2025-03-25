using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Model.Scheduler;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Tasks.Implementations
{
    public class EquipmentRecycleResetTask : DailyTask
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipmentRecycleResetTask));

        public EquipmentRecycleResetTask(uint hour, uint minute) : base(TaskType.EquipmentRecycleReset, hour, minute)
        {
        }

        public override bool IsEnabled(DdonGameServer server)
        {
            return true;
        }

        public override void RunTask(DdonGameServer server)
        {
            Logger.Info("Equipment Recycling Reset amount reset");
            server.Database.ResetRecyleEquipmentRecords();
        }
    }
}

