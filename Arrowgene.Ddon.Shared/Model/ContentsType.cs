using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public enum ContentsType : uint
    {
        ExtremeMission = 1,
        ClanDungeon = 2,
        Unkown3 = 3,
        ChainDungeon = 4,
        // May be more types

#if false
        None = 0xffffffff,
        Cycle = 0,
        End = 1,
        Unk2 = 2,
        Unk3 = 3,
        Chain = 4

        Unknown = 0,
        Begin = 1,
        WorldQuest = 2,
        Cycle = 3,
        End = 4,
        QuickPartyMainQuest = 6,
        QuickPartyArea = 7,
        Large = 8
#endif
    }
}
