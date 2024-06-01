using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public enum QuestProgressState : uint
    {
        Unknown = 0,
        Accepted = 1,
        InProgress = 2,
        Cleared = 2,
        Failed = 3,
        Complete = 4,
        ReportToNpc = 5,
    }
}
