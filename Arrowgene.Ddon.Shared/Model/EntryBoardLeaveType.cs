using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public enum EntryBoardLeaveType : uint
    {
        EntryBoardRemoved = 0,
        EntryBoardDisolved = 1,
        EntryBoardTimeUp = 2,
        EntryBoardReadyTimeUp = 3,
        MemberReadyTimeup = 4,
        EntryBoardAlarm = 5,
    }
}
