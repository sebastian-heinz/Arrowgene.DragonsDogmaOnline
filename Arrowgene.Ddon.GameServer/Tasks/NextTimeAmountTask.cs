using Arrowgene.Ddon.Shared.Model.Scheduler;
using System;

namespace Arrowgene.Ddon.GameServer.Tasks
{
    public abstract class NextTimeAmountTask : SchedulerTask
    {
        public uint Hours { get; }
        public uint Minutes { get; }

        public NextTimeAmountTask(TaskType type, uint hours, uint minutes = 0) : base(ScheduleInterval.Hourly, type)
        {
            Hours = hours;
            Minutes = minutes;
        }

        public override long NextTimestamp()
        {
            return new DateTimeOffset(DateTime.Now.AddHours(Hours).AddMinutes(Minutes)).ToUnixTimeSeconds();
        }

        public override string TaskTypeName()
        {
            return "Next Time Amount Task";
        }
    }
}
