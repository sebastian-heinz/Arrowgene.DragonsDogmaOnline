using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public enum QuestBlockType : uint
    {
        None = 0,
        NpcTalkAndOrder,
        NpcTouchAndOrder,
        QuestNpcTalkAndOrder,
        DiscoverEnemy,
        KillGroup,
        TalkToNpc,
        DeliverItems,
        SeekOutEnemiesAtMarkedLocation,
        CollectItem,
        MyQstFlags,
        IsStageNo,
        IsQuestOrdered,
        Raw,
        DummyBlock,
        DummyBlockNoProgress,
        End
    }
}
