using Arrowgene.Ddon.Shared.Model.Scheduler;
using System;

namespace Arrowgene.Ddon.GameServer.Tasks
{
    public abstract class HourlyTask : SchedulerTask
    {
        /// <summary>
        /// Task which is always scheduled to run on the hour in the timezone of the server currently running.
        /// </summary>
        /// <param name="type">The task type associated with this task</param>
        public HourlyTask(TaskType type) : base(ScheduleInterval.Hourly, type)
        {
        }

        public override long NextTimestamp()
        {
            var now = DateTime.Now;
            var next = now.AddHours(1);
            var nextTime = new DateTime(next.Year, next.Month, next.Day, next.Hour, 0, 0);
            return new DateTimeOffset(now.Add(nextTime - now)).ToUnixTimeSeconds();
        }

        public override string TaskTypeName()
        {
            return "Hourly Task";
        }
    }
}
