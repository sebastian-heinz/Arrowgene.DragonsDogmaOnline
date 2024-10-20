using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class QuestProgress
    {
        public uint CharacterCommonId { get; set; }
        public uint QuestScheduleId { get; set; }
        public QuestType QuestType { get; set; }
        public uint Step { get; set; }
    }

    public enum QuestProgressStatus : uint
    {
        ExecuteCommand = 0,
        QuestProgress = 1,
        WaitProgress = 2,
        ProcessEnd = 3,
        Error = 4
    }
}
