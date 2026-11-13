/**
 * @brief Magick Called to the Deepest Depths
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.MagickCalledToTheDeepestDepths; // Schedule ID: 1677886848
    public override ushort RecommendedLevel => 100;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;
    public override StageInfo StageInfo => Stage.FirefallMountainCampsite;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.AreaTrialOrMission;
    public override bool Enabled => false;

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.PersonalQuestCleared(QuestId.UrtecaMountainsTrialATrickleInTheDarkness));
        AddQuestOrderCondition(QuestOrderCondition.RequiredItemRank(114));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 111525);
        AddWalletReward(WalletType.Gold, 11000);
        AddWalletReward(WalletType.RiftPoints, 2000);

        AddFixedItemReward(ItemId.AncientWheatEar, 1);
        AddFixedItemReward(ItemId.DragonTemplesBlessedCloth, 1);
        AddFixedItemReward(ItemId.SacredFlameSteel, 1);
    }

    private class EnemyGroupId
    {
        public const uint Set8295 = 8295;
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Set8295, Stage.SacredFlamePath0, 1, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.Ushumgal, 100, 105000, 0)
                .SetIsBoss(true),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsTutorialQuestClear(QuestId.UrtecaMountainsTrialATrickleInTheDarkness);
        process0.AddNpcTalkAndOrderBlock(Stage.FirefallMountainCampsite, NpcId.Bacias, 30176);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Accept, EnemyGroupId.Set8295)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.SacredFlamePath.HallOfTheMagicHornsBoulder)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.SacredFlamePath.HallOfTheMagicHornsWarp)
            .AddResultCmdQstTalkChg(NpcId.Bacias, 30177);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Set8295, resetGroup: false);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FirefallMountainCampsite, NpcId.Bacias, 30178);
        process0.AddProcessEndBlock(true)
            .AddResultCmdCallGreatPurpose(6, 13); // Should display the achievement name this time, but how?
    }
}

return new ScriptedQuest();
