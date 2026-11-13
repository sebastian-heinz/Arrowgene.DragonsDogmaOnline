using Arrowgene.Ddon.Shared.Model.Scheduler;
using System;

namespace Arrowgene.Ddon.GameServer.Tasks
{
    public abstract class SchedulerTask
    {
        public TaskType Type { get; }
        public ScheduleInterval Interval { get; }

        /// <summary>
        /// Returns the UTC offset to use when computing the next fire time. Evaluated fresh on
        /// every NextTimestamp() call so DST transitions are picked up automatically when a
        /// ServerTimeZoneId is configured.
        /// </summary>
        public TimeSpan Offset => GetOffset();

        /// <summary>
        /// Delegate that produces the current UTC offset. Set by ScheduleManager using the
        /// server's ServerTimeZoneId / ServerUtcOffset settings.
        /// </summary>
        public Func<TimeSpan> GetOffset { get; set; } = () => TimeSpan.Zero;

        /// <param name="interval">Hint for the type of interval this task is expected to occur at.</param>
        /// <param name="type">
        ///     The task type which is stored in the DB and used to resume the scheduler
        ///     timer when the head server starts.
        /// </param>
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
        /// <param name="server">The head server object</param>
        public abstract void RunTask(DdonGameServer server);

        /// <summary>
        /// Generates the next unix timestamp to store in the database for the task.
        /// </summary>
        /// <returns>Returns the unix timestamp which represents the next time this task should activate.</returns>
        public abstract long NextTimestamp();

        /// <summary>
        /// By default, all tasks will return that they are enabled. A child class can override
        /// this function to provide custom checks for enablement.
        /// </summary>
        /// <param name="server">The head server object</param>
        /// <returns>Returns true if this task is enabled, otherwise false.</returns>
        public virtual bool IsEnabled(DdonGameServer server)
        {
            return true;
        }

        /// <summary>
        /// Returns the name of the task type.
        /// </summary>
        /// <returns></returns>
        public abstract string TaskTypeName();

        public virtual string TaskName()
        {
            return Type.ToString();
        }
    }
}
