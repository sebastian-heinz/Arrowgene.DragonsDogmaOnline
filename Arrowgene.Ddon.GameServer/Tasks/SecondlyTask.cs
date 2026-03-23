using Arrowgene.Ddon.Shared.Model.Scheduler;
using System;

namespace Arrowgene.Ddon.GameServer.Tasks
{
    public abstract class SecondlyTask : SchedulerTask
    {
        public uint Seconds { get; }

        /// <summary>
        /// Calculates the next timestamp for this task in unix seconds
        /// </summary>
        /// <returns>Returns the next timestamp in unix seconds</returns>
        public SecondlyTask(TaskType taskType, uint seconds) : base(ScheduleInterval.Secondly, taskType)
        {
            Seconds = seconds;
        }

        /// <summary>
        /// Calculates the next timestamp for this task in unix seconds
        /// </summary>
        /// <returns>Returns the next timestamp in unix seconds</returns>
        public override long NextTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Seconds;
        }

        public override string TaskTypeName()
        {
            return "Seconds Amount Task";
        }
    }
}