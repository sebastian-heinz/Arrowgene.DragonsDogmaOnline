using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public enum QuestRewardType : uint
    {
        Unknown = 0,
        Fixed = 1,
        Select = 2,
        Undiscovery = 3,
        Random = 4,
        Repeat = 5,
        Switch = 6,
        Border = 7,
        Ranking = 8,
        Charge = 9,
        RegionBreak = 10,
        FixedFirst = 11,
        FixedSecond = 12,
        FixedMemberFirst = 13,
        ProgressBonus = 14,
        Num = 15,
    }
}
