using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public enum ContentsType : uint
    {
        Unknown = 0,
        Begin = 1,
        WorldQuest = 2,
        Cycle = 3,
        End = 4,
        QuickPartyMainQuest = 6,
        QuickPartyArea = 7,
        Large = 8
    }
}
