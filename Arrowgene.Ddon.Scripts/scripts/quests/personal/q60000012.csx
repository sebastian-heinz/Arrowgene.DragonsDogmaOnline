/**
 * @brief The Arisen's Abilities
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.TheArisensAbilities;
    public override ushort RecommendedLevel => 1;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.QuestUsefulForAdventure;

    private static class QstLayoutFlag
    {
        public const uint Fabio0 = 1018; // In front of audience chamber NPCセット_ファビオ（謁見前) (Group=1, UnitNo=1)
        public const uint Fabio1 = 1716; // In front of storage box NPCセットーファビオ（保管BOX前）(Group=2, UnitNo=1)
        public const uint Fabio3 = 957; // In front of Board - NPCセット-ファビオ（ボード前）(Group=3, UnitNo=1)
    }

    protected override void InitializeRewards()
    {
        AddFixedItemReward(ItemId.LeatherCloak0, 1);
        AddPointReward(PointType.ExperiencePoints, 450);
        AddWalletReward(WalletType.Gold, 300);
        AddWalletReward(WalletType.RiftPoints, 100);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsMainQuestClear(QuestId.TheSlumberingGod);
        process0.AddIsStageNoBlock(QuestAnnounceType.Accept, Stage.TheWhiteDragonTemple0);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.TheWhiteDragonTemple0, 5, 3)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Fabio0);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Archibald0, 13697)
            .AddResultCmdTutorialDialog(TutorialId.PriorityQuests);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddCheckCmdIsSetPlayerSkill()
            .AddResultCmdReleaseAnnounce(ContentsRelease.ChangeVocations, TutorialId.AcquiringCombatManuevers, QuestFlags.NpcFunctions.VocationArts);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Archibald0, 14625);
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, 3, 1, NpcId.Fabio0, 13701)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.Fabio0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Fabio3);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddResultCmdReleaseAnnounce(ContentsRelease.QuestBoard, TutorialId.BoardQuest)
            .AddCheckCmdIsAcceptLightQuest();
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddCheckCmdIsClearLightQuest();
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, 2, 1, NpcId.Fabio0, 14118)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, QstLayoutFlag.Fabio3)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, QstLayoutFlag.Fabio1);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddResultCmdTutorialDialog(TutorialId.StorageBox)
            .AddCheckCmdIsOpenWarehouseReward();
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, 2, 1, NpcId.Fabio0, 10978);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0); // This is trying to access the online store so skip it
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, 2, 1, NpcId.Fabio0, 22814);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.TheWhiteDragonTemple0)
            .AddResultCmdReleaseAnnounce(ContentsRelease.RiftTeleport, TutorialId.DragonsKeystoneandRiftTeleport)
            .AddResultCmdTutorialDialog(TutorialId.MailConfirmation);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
