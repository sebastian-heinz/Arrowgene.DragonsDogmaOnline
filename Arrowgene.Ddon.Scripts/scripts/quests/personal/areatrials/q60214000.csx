/**
 * @brief Elan Water Grove Trial: The Howling from the Dark (Elan Water Grove AR4)
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60214000;
    public override ushort RecommendedLevel => 70;
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
        public const uint TargetTitle = 1247; // Interfering <name>
    }

    private class TargetEnemyId
    {
        public const EnemyId Target = EnemyId.Warg;
    }


    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 15350);
        AddWalletReward(WalletType.Gold, 4618);
        AddWalletReward(WalletType.RiftPoints, 600);

        AddFixedItemReward(ItemId.OldGeranium, 10);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.MainGroup + 0, Stage.ElanWaterGrove, 9, QuestEnemyPlacementType.Manual, 
        Enumerable.Range(8, 5).Select(x => LibDdon.Enemy.CreateAuto(TargetEnemyId.Target, RecommendedLevel, (byte)x)
                .SetNamedEnemyParams(NamedParamId.TargetTitle)).ToList());
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);

        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdCheckAreaRank(QuestAreaId.ElanWaterGrove, 3);

        process0.AddNpcTalkAndOrderBlock(Stage.ProtectorsRetreat, NpcId.Musel0, 19310);

        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.ElanWaterGrove, NpcId.Enna, 19312)
            .AddResultCmdQstTalkChg(NpcId.Musel0, 19311);

        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.MainGroup)
            .AddResultCmdQstTalkChg(NpcId.Musel0, 19311)
            .AddResultCmdQstTalkChg(NpcId.Enna, 19313);

        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.MainGroup, false)
            .AddResultCmdSetAnnounce(QuestAnnounceType.Caution)
            .AddResultCmdQstTalkChg(NpcId.Musel0, 19311)
            .AddResultCmdQstTalkChg(NpcId.Enna, 19313);

        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.ElanWaterGrove, NpcId.Enna, 19314)
            .AddResultCmdQstTalkChg(NpcId.Musel0, 19311);

        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
