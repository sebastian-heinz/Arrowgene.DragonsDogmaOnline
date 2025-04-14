/**
 * @brief Elan Water Grove Trial: A Silvery Shadow Swoops (Elan Water Grove AR10)
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60214002;
    public override ushort RecommendedLevel => 75;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;
    public override StageInfo StageInfo => Stage.ProtectorsRetreat;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.AreaTrialOrMission;

    private class EnemyGroupId
    {
        public const uint MainGroup = 1;
    }

    private class NamedParamId
    {
        public const uint TargetTitle = 1249; // Unbidden White Eagle
    }

    private class TargetEnemyId
    {
        public const EnemyId Target = EnemyId.WhiteGriffin0;
    }

    private class NpcText
    {
        public const int MuselIntro = 19315;
        public const int MuselIdle = 19316;

        public const int DirisIntro = 19317;
        public const int DirisIdle = 19318;
        public const int DirisReturn = 19319;
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 24350);
        AddWalletReward(WalletType.Gold, 8987);
        AddWalletReward(WalletType.RiftPoints, 1330);

        AddFixedItemReward(ItemId.ElanMedal, 1);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.MainGroup, Stage.JungleLimestoneCaves, 4, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(TargetEnemyId.Target, RecommendedLevel, 1, isBoss:true)
                .SetNamedEnemyParams(NamedParamId.TargetTitle)
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);

        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdCheckAreaRank(QuestAreaId.ElanWaterGrove, 9);

        process0.AddNpcTalkAndOrderBlock(Stage.ProtectorsRetreat, NpcId.Musel0, NpcText.MuselIntro);

        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.ProtectorsRetreat, NpcId.Dirith1, NpcText.DirisIntro)
            .AddResultCmdQstTalkChg(NpcId.Musel0, NpcText.MuselIdle);

        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.MainGroup)
            .AddResultCmdQstTalkChg(NpcId.Musel0, NpcText.MuselIdle)
            .AddResultCmdQstTalkChg(NpcId.Dirith1, NpcText.DirisIdle);

        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.MainGroup, false)
            .AddResultCmdSetAnnounce(QuestAnnounceType.Caution);

        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.ProtectorsRetreat, NpcId.Dirith1, NpcText.DirisReturn)
            .AddResultCmdQstTalkChg(NpcId.Musel0, NpcText.MuselIdle);

        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
