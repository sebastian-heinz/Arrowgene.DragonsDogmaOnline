using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Scheduler
{
    public class SchedulerTaskEntry
    {
        public TaskType Type { get; set; }
        public long Timestamp { get; set; }
    }
}
