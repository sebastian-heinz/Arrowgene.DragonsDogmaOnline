using Arrowgene.Ddon.Shared.Model.Scheduler;
using System;

namespace Arrowgene.Ddon.GameServer.Tasks
{
    public abstract class DailyTask : SchedulerTask
    {
        public uint Hour { get; }
        public uint Minute { get; }

        public DailyTask(TaskType scheduleType, uint hour, uint minute) : base(ScheduleInterval.Daily, scheduleType)
        {
            Hour = hour;
            Minute = minute;
        }

        public override long NextTimestamp()
        {
            var tomorrow = DateTime.Today.AddDays(1);
            return new DateTimeOffset(new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, (int)Hour, (int)Minute, 0)).ToUnixTimeSeconds();
        }

        public override string TaskTypeName()
        {
            return "Daily Task";
        }
    }
}
