using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public enum QuestProcess : byte
    {
        ExecuteCommand = 0,
        QuestProgress = 1,
        WaitProgress = 2,
        ProcessEnd = 3,
        Error = 4
    }
}
