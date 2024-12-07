using Arrowgene.Ddon.Shared.Model.Scheduler;
using System;

namespace Arrowgene.Ddon.GameServer.Tasks
{
    public abstract class HourlyTask : SchedulerTask
    {
        public HourlyTask(TaskType type) : base(ScheduleInterval.Hourly, type)
        {
        }

        public override long NextTimestamp()
        {
            return new DateTimeOffset(DateTime.Now.AddHours(1)).ToUnixTimeSeconds();
        }
    }
}
