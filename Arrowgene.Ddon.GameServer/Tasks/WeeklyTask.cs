using Arrowgene.Ddon.GameServer.Utils;
using Arrowgene.Ddon.Shared.Model.Scheduler;
using System;

namespace Arrowgene.Ddon.GameServer.Tasks
{
    public abstract class WeeklyTask : SchedulerTask
    {
        public DayOfWeek Day { get; }
        public uint Hour { get; }
        public uint Minute { get; }

        /// <summary>
        /// Creates a task which runs on a weekly cadence.
        /// Uses the timezone of the head server when calculating times.
        /// </summary>
        /// <param name="scheduleType">The type of event this is associated with</param>
        /// <param name="day">The day during the week the reset should occur.</param>
        /// <param name="hour">The hour the reset should occur in a 24 hour format</param>
        public WeeklyTask(TaskType scheduleType, DayOfWeek day, uint hour, uint minute) : base(ScheduleInterval.Weekly, scheduleType)
        {
            Day = day;
            Hour = hour;
            Minute = minute;
        }

        /// <summary>
        /// Calculates the next timestamp for this task in unix seconds
        /// </summary>
        /// <returns>Returns the next timestamp in unix seconds</returns>
        public override long NextTimestamp()
        {
            var nextDate = DateUtils.GetNextWeekday(DateTime.Today.AddDays(1), Day);
            return new DateTimeOffset(new DateTime(nextDate.Year, nextDate.Month, nextDate.Day, (int)Hour, (int)Minute, 0)).ToUnixTimeSeconds();
        }

        public override string TaskTypeName()
        {
            return "Weekly Task";
        }
    }
}
