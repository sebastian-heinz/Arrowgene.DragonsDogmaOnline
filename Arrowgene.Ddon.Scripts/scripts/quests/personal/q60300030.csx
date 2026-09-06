/**
 * @brief Save the Royal Family Cook
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60300030; // Schedule ID: 1673531136
    public override ushort RecommendedLevel => 85;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.FortThines1;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.QuestUsefulForAdventure;
    public override bool Enabled => false;

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 42000);
        AddWalletReward(WalletType.Gold, 11000);
        AddWalletReward(WalletType.RiftPoints, 2000);

        AddFixedItemReward(ItemId.RoyalCrestMedalRathniteDistrict, 3);
        AddFixedItemReward(ItemId.CobaltAlloyIngot, 1);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsMainQuestClear(QuestId.ThePrincesWhereabouts);
        process0.AddNpcTalkAndOrderBlock(Stage.FortThines1, NpcId.Zeki, 25937);
        process0.AddIsStageNoBlock(QuestAnnounceType.Accept, Stage.FortThinesGreatDiningHall)
            .AddResultCmdQstTalkChg(NpcId.Zeki, 25938);
        process0.AddTalkToNpcBlock(QuestAnnounceType.None, Stage.FortThinesGreatDiningHall, NpcId.Carrie2, 25939)
            .AddResultCmdSetOmState(Stage.FortThinesGreatDiningHall, 0, 0, 0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 6412);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddResultCmdQstTalkChg(NpcId.Carrie2, 25940)
            .AddCheckCommands([
                QuestManager.CheckCommand.TalkNpcChoice(448, NpcId.Carrie2, 0)
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.TalkNpc(448, NpcId.Carrie2),
                QuestManager.CheckCommand.DummyNotProgress()
            ]);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.FortThinesGreatDiningHall, 0, 3);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FortThinesGreatDiningHall, NpcId.Carrie2, 27028)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 6955)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 6412)
            .AddResultCmdSetOmState(Stage.FortThinesGreatDiningHall, 1, 0, 0);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FortThines1, NpcId.Zeki, 26064)
            .AddResultCmdQstTalkChg(NpcId.Carrie2, 26065);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, 6955)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.FortThinesGreatDiningHall.Carrie)
            .AddResultCmdReleaseAnnounce(ContentsRelease.CooperatorsoftheRoyalFamily)
			.AddResultCmdCallGreatPurpose(6, 3)
            .AddCheckCommands([
                QuestManager.CheckCommand.TutorialTalkNpc(448, NpcId.Carrie2)
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.TouchActToNpc(448, NpcId.Carrie2),
                QuestManager.CheckCommand.DummyNotProgress()
            ]);
        process0.AddProcessEndBlock(true)
            .AddResultCmdReleaseAnnounce(ContentsRelease.None, TutorialId.AchievementsRoyalFamilyRestoration);
    }
}

return new ScriptedQuest();
