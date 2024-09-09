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
        IsGatherPartyInStage,
        DiscoverEnemy,
        KillGroup,
        SpawnGroup,
        WeakenGroup,
        DestroyGroup,
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
        KillTargetEnemies,
        ExtendTime,
        Raw,
        DummyBlock,
        DummyBlockNoProgress,
        End
    }
}
