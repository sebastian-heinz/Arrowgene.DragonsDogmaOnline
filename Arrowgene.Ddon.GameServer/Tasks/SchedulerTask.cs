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

        /// <summary>
        /// Runs on the head server. Should deal with things like modifying the database.
        /// Should use the RPC manage if it is required to update clients on different channels
        /// or send annoucements to players.
        /// </summary>
        /// <param name="server"></param>
        public abstract void RunTask(DdonGameServer server);
        public abstract long NextTimestamp();

        public virtual bool IsEnabled(DdonGameServer server)
        {
            return true;
        }
    }
}
