/**
 * @brief Kingal Canyon Trial: Dancing Shadows on the Water (Kingal Canyon AR4)
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60217000;
    public override ushort RecommendedLevel => 70;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;
    public override StageInfo StageInfo => Stage.GlyndwrCentrum;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.AreaTrialOrMission;

    private class EnemyGroupId
    {
        public const uint MainGroup = 1;
    }

    private class NamedParamId
    {
        public const uint TargetTitle = 1253; // Riverside
    }

    private class TargetEnemyId
    {
        public const EnemyId Target = EnemyId.ShadowHarpy;
    }

    private class NpcText
    {
        public const int AMIntro = 19330;
        public const int AMIdle = 19331;

        public const int ReqIntro = 19332;
        public const int ReqIdle = 19333;
        public const int ReqReturn = 19334;
    }

    private class QuestNpcs
    {
        public const NpcId AreaMaster = NpcId.Ciaran;
        public const NpcId Requester = NpcId.Oengus;
    }

    private class QuestStages 
    {
        public static readonly StageInfo AreaMasterStage = Stage.GlyndwrCentrum;
        public static readonly StageInfo TargetStage = Stage.KingalCanyon;
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 15350);
        AddWalletReward(WalletType.Gold, 4618);
        AddWalletReward(WalletType.RiftPoints, 600);

        AddFixedItemReward(ItemId.RingingOre, 10);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.MainGroup, QuestStages.TargetStage, 10, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(TargetEnemyId.Target, RecommendedLevel, 16)
                .SetNamedEnemyParams(NamedParamId.TargetTitle),
            LibDdon.Enemy.CreateAuto(TargetEnemyId.Target, RecommendedLevel, 15)
                .SetNamedEnemyParams(NamedParamId.TargetTitle),
            LibDdon.Enemy.CreateAuto(TargetEnemyId.Target, RecommendedLevel, 14)
                .SetNamedEnemyParams(NamedParamId.TargetTitle),
            LibDdon.Enemy.CreateAuto(TargetEnemyId.Target, RecommendedLevel, 13)
                .SetNamedEnemyParams(NamedParamId.TargetTitle),
            LibDdon.Enemy.CreateAuto(TargetEnemyId.Target, RecommendedLevel, 12)
                .SetNamedEnemyParams(NamedParamId.TargetTitle)
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);

        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdCheckAreaRank(QuestAreaId.KingalCanyon, 3);

        // "Get the details of the case from the Area Master and try to solve the problem"
        process0.AddNpcTalkAndOrderBlock(QuestStages.AreaMasterStage, QuestNpcs.AreaMaster, NpcText.AMIntro);

        // Confirm details from <NPC 2705> in <SPOT 879>
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, QuestStages.TargetStage, QuestNpcs.Requester, NpcText.ReqIntro)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle);

        // Investigate problem locations
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.MainGroup, true)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle)
            .AddResultCmdQstTalkChg(QuestNpcs.Requester, NpcText.ReqIdle);

        // Defeat the demons that are the source of evil
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.MainGroup, false)
            .AddResultCmdSetAnnounce(QuestAnnounceType.Caution);

        // Report to <NPC 2705> in <SPOT 879>
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, QuestStages.TargetStage, QuestNpcs.Requester, NpcText.ReqReturn)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle);

        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
