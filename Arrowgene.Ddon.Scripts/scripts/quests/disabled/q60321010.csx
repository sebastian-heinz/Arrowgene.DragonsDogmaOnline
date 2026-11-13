/**
 * @brief Urteca Mountains Trial: A Trickle in the Darkness
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.UrtecaMountainsTrialATrickleInTheDarkness; // Schedule ID: 1677886720
    public override ushort RecommendedLevel => 100;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;
    public override bool? EnableCancel => true;
    public override StageInfo StageInfo => Stage.FirefallMountainCampsite;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.AreaTrialOrMission;
    public override bool Enabled => false;

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.SoloWithPawns(3));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheRelicsOfTheFirstKing));
        AddQuestOrderCondition(QuestOrderCondition.RequiredItemRank(114));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 111525);
        AddWalletReward(WalletType.Gold, 11000);
        AddWalletReward(WalletType.RiftPoints, 2000);

        AddFixedItemReward(ItemId.RoyalCrestMedalUrtecaDistrict, 3);
        AddFixedItemReward(ItemId.VolcanicTuff, 1);
        AddFixedItemReward(ItemId.VolcanicMountainAsh, 1);
    }

    private class EnemyGroupId
    {
        public const uint Set8028 = 8028;
        public const uint Set8029 = 8029;
        public const uint Set8030 = 8030;
        public const uint Set8031 = 8031;
        public const uint Set8198 = 8198;
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.Set8028, Stage.SacredFlamePath0, 7, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.SkeletonSorcerer0, 100, 408, 3),
            LibDdon.Enemy.Create(EnemyId.SkeletonSorcerer0, 100, 408, 4),
            LibDdon.Enemy.Create(EnemyId.SkeletonSorcerer0, 100, 408, 5),
            LibDdon.Enemy.Create(EnemyId.SkeletonSorcerer0, 100, 408, 6),
        });

        AddEnemies(EnemyGroupId.Set8029, Stage.SacredFlamePath0, 8, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.SkeletonMage0, 100, 360, 3),
            LibDdon.Enemy.Create(EnemyId.GoblinShaman, 100, 750, 4),
            LibDdon.Enemy.Create(EnemyId.GoblinShaman, 100, 750, 5),
            LibDdon.Enemy.Create(EnemyId.SkeletonMage0, 100, 360, 6),
        });

        AddEnemies(EnemyGroupId.Set8030, Stage.SacredFlamePath0, 14, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.SkeletonSorcerer0, 100, 408, 5)
                .SetStartThinkTblNo(8)
                .SetIsManualSet(true)
                .SetIsRequired(false),
            LibDdon.Enemy.Create(EnemyId.SkeletonSorcerer0, 100, 408, 6)
                .SetStartThinkTblNo(8)
                .SetIsManualSet(true)
                .SetIsRequired(false),
            LibDdon.Enemy.Create(EnemyId.SkeletonSorcerer0, 100, 408, 7)
                .SetStartThinkTblNo(8)
                .SetIsManualSet(true)
                .SetIsRequired(false),
            LibDdon.Enemy.Create(EnemyId.SkeletonSorcerer0, 100, 408, 8)
                .SetStartThinkTblNo(8)
                .SetIsManualSet(true)
                .SetIsRequired(false),
            LibDdon.Enemy.Create(EnemyId.SkeletonSorcerer0, 100, 408, 9)
                .SetStartThinkTblNo(8)
                .SetIsManualSet(true)
                .SetIsRequired(false),
            LibDdon.Enemy.Create(EnemyId.SkeletonSorcerer0, 100, 408, 10)
                .SetStartThinkTblNo(8)
                .SetIsManualSet(true)
                .SetIsRequired(false),
            LibDdon.Enemy.Create(EnemyId.SkeletonSorcerer0, 100, 408, 11)
                .SetStartThinkTblNo(8)
                .SetIsManualSet(true)
                .SetIsRequired(false),
            LibDdon.Enemy.Create(EnemyId.SkeletonSorcerer0, 100, 408, 12)
                .SetStartThinkTblNo(8)
                .SetIsManualSet(true)
                .SetIsRequired(false),
            LibDdon.Enemy.Create(EnemyId.EmpressGhost, 100, 6400, 13)
                .SetIsBoss(true),
        });

        // Apparently unused?
        AddEnemies(EnemyGroupId.Set8031, Stage.SacredFlamePath0, 15, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.BlazeChimera, 100, 0, 11)
                .SetIsBoss(true),
            LibDdon.Enemy.Create(EnemyId.HeavySoldierDwarfOrc, 100, 0, 12),
            LibDdon.Enemy.Create(EnemyId.HeavySoldierDwarfOrc, 100, 0, 13),
            LibDdon.Enemy.Create(EnemyId.HeavySoldierDwarfOrc, 100, 0, 14),
            LibDdon.Enemy.Create(EnemyId.HeavySoldierDwarfOrc, 100, 0, 15),
        });

        // Seen after the summoned skeletons are defeated, but in only one video. What are the requirements?
        AddEnemies(EnemyGroupId.Set8198, Stage.SacredFlamePath0, 26, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.Create(EnemyId.GoblinShaman, 100, 750, 0),
            LibDdon.Enemy.Create(EnemyId.GoblinShaman, 100, 750, 1),
            LibDdon.Enemy.Create(EnemyId.GoblinShaman, 100, 750, 2),
            LibDdon.Enemy.Create(EnemyId.GoblinShaman, 100, 750, 3),
            LibDdon.Enemy.Create(EnemyId.GoblinShaman, 100, 750, 4),
            LibDdon.Enemy.Create(EnemyId.GoblinShaman, 100, 750, 5),
            LibDdon.Enemy.Create(EnemyId.GoblinShaman, 100, 750, 6),
            LibDdon.Enemy.Create(EnemyId.GoblinShaman, 100, 750, 7),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdCheckAreaRank(QuestAreaId.UrtecaMountains, 9);
        process0.AddNpcTalkAndOrderBlock(Stage.FirefallMountainCampsite, NpcId.Bacias, 30160);
        process0.AddRawBlock(QuestAnnounceType.Accept)
            .AddCheckCmdTouchActToNpc(Stage.FirefallMountainCampsite, NpcId.Bacias);
        process0.AddEventAfterJumpContinueBlock(QuestAnnounceType.None, Stage.SacredFlamePath0, 0, 0);
        process0.AddStageJumpBlock(QuestAnnounceType.None, Stage.FirefallMountainCampsite, 5);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FirefallMountainCampsite, NpcId.Bacias, 30161);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.Set8028)
            .AddResultCmdQstTalkChg(NpcId.Bacias, 30162);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Set8028, resetGroup: false);
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdQstLayoutFlagOn(8031)
            .AddCheckCmdOmReleaseTouchRadius(Stage.SacredFlamePath0, 1, 0);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Set8029)
            .AddResultCmdQstLayoutFlagOff(8031);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Set8029, resetGroup: false);
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdQstLayoutFlagOn(8154)
            .AddCheckCmdOmReleaseTouchRadius(Stage.SacredFlamePath0, 2, 0);
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdQstLayoutFlagOff(8154)
            .AddResultCmdQstLayoutFlagOn(8155)
            .AddCheckCmdOmReleaseTouchRadius(Stage.SacredFlamePath0, 3, 0);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.Set8030)
            .AddResultCmdQstLayoutFlagOff(8155)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, 100721);
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, 100722)
            .AddResultCmdQstLayoutFlagOn(8032)
            .AddCheckCmdQuestOmReleaseTouch(Stage.SacredFlamePath0, 0, 0);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FirefallMountainCampsite, NpcId.Bacias, 30163)
            .AddResultCmdQstLayoutFlagOff(8032);
        process0.AddProcessEndBlock(true)
            .AddResultCmdCallGreatPurpose(6, 13);
    }
}

return new ScriptedQuest();
