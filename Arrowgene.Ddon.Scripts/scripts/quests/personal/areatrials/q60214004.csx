/**
 * @brief Elan Water Grove Trial：Rampaging Green Tree (Elan Water Grove AR15)
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60214004;
    public override ushort RecommendedLevel => 80;
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
        public const uint TargetTitle = 1272; // <name> the Village Crusher
    }

    private class TargetEnemyId
    {
        public const EnemyId Target = EnemyId.PhindymianEnt0;
    }

    private class NpcText
    {
        public const int MuselIntro = 19345;
        public const int MuselIdle = 19346;

        public const int DirisIntro = 19347;
        public const int DirisIdle = 19348;
        public const int MuselReturn = 19349;
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 40350);
        AddWalletReward(WalletType.Gold, 10277);
        AddWalletReward(WalletType.RiftPoints, 1880);

        AddFixedItemReward(ItemId.ElanMedal, 1);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.MainGroup + 0, Stage.ElanWaterGrove, 15, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(TargetEnemyId.Target, RecommendedLevel, 20, isBoss:true)
                .SetNamedEnemyParams(NamedParamId.TargetTitle)
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);

        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdCheckAreaRank(QuestAreaId.ElanWaterGrove, 14);

        // Get the details of the case from the Area Master and try to solve the problem
        process0.AddNpcTalkAndOrderBlock(Stage.ProtectorsRetreat, NpcId.Musel0, NpcText.MuselIntro);

        // Confirm details from <NPC 559> in <SPOT 876>
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.ProtectorsRetreat, NpcId.Dirith1, NpcText.DirisIntro)
            .AddResultCmdQstTalkChg(NpcId.Musel0, NpcText.MuselIdle);

        // Investigate problem locations
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.MainGroup)
            .AddResultCmdQstTalkChg(NpcId.Musel0, NpcText.MuselIdle)
            .AddResultCmdQstTalkChg(NpcId.Dirith1, NpcText.DirisIdle);

        // Defeat the demons that are the source of evil
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.MainGroup, false)
            .AddResultCmdSetAnnounce(QuestAnnounceType.Caution);

        // Report to <NPC 4509> in <SPOT 876>
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.ProtectorsRetreat, NpcId.Musel0, NpcText.MuselReturn);

        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
