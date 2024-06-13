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
        PartyGather,
        DiscoverEnemy,
        KillGroup,
        SpawnGroup,
        TalkToNpc,
        NewTalkToNpc,
        NewNpcTalkAndOrder,
        DeliverItems,
        SeekOutEnemiesAtMarkedLocation,
        CollectItem,
        OmInteractEvent,
        MyQstFlags,
        IsStageNo,
        IsQuestOrdered,
        PlayEvent,
        Raw,
        DummyBlock,
        DummyBlockNoProgress,
        End
    }
}
