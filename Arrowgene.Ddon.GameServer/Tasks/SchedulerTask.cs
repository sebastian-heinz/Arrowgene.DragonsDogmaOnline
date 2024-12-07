using Arrowgene.Ddon.Shared.Model.Scheduler;

namespace Arrowgene.Ddon.GameServer.Tasks
{
    public abstract class SchedulerTask
    {
        public TaskType Type { get; }
        public ScheduleInterval Interval { get; }

        public SchedulerTask(ScheduleInterval interval, TaskType type)
        {
            Type = type;
            Interval = interval;
        }

        public abstract void RunTask(DdonGameServer server);
        public abstract long NextTimestamp();

        public virtual bool IsEnabled(DdonGameServer server)
        {
            return true;
        }
    }
}
