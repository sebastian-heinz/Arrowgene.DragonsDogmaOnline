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
        NewDeliverItems,
        SeekOutEnemiesAtMarkedLocation,
        CollectItem,
        OmInteractEvent,
        MyQstFlags,
        IsStageNo,
        IsQuestOrdered,
        PlayEvent,
        KillTargetEnemies,
        ExtendTime,
        ReturnCheckpoint,
        Raw,
        DummyBlock,
        DummyBlockNoProgress,
        End
    }
}
