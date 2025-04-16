/**
 * @brief Morrow Forest Trial: Flocking Apprehension (Morrow Forest AR4)
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60216000;
    public override ushort RecommendedLevel => 70;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;
    public override StageInfo StageInfo => Stage.MorfaulCentrum;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.AreaTrialOrMission;

    private class EnemyGroupId
    {
        public const uint MainGroup = 1;
    }

    private class NamedParamId
    {
        public const uint TargetTitle = 1250; // Flock <name>
    }

    private class TargetEnemyId
    {
        public const EnemyId Target = EnemyId.SeverelyInfectedStymphalides;
    }

    private class NpcText
    {
        public const int AMIntro = 19320;
        public const int AMIdle = 19321;

        public const int ReqIntro = 19322;
        public const int ReqIdle = 19323;
        public const int ReqReturn = 19324;
    }

    private class QuestNpcs
    {
        public const NpcId AreaMaster = NpcId.Arthfael;
        public const NpcId Requester = NpcId.Zolta;
    }

    private class QuestStages 
    {
        public static readonly StageInfo AreaMasterStage = Stage.MorfaulCentrum;
        public static readonly StageInfo TargetStage = Stage.MorrowForest;
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 15350);
        AddWalletReward(WalletType.Gold, 4618);
        AddWalletReward(WalletType.RiftPoints, 600);

        AddFixedItemReward(ItemId.RottenTree, 10);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.MainGroup, QuestStages.TargetStage, 10, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(TargetEnemyId.Target, RecommendedLevel, 0)
                .SetNamedEnemyParams(NamedParamId.TargetTitle)
                .SetInfectionType(2), // Severe, 重度
            LibDdon.Enemy.CreateAuto(TargetEnemyId.Target, RecommendedLevel, 1)
                .SetNamedEnemyParams(NamedParamId.TargetTitle)
                .SetInfectionType(2),
            LibDdon.Enemy.CreateAuto(TargetEnemyId.Target, RecommendedLevel, 2)
                .SetNamedEnemyParams(NamedParamId.TargetTitle)
                .SetInfectionType(2),
            LibDdon.Enemy.CreateAuto(TargetEnemyId.Target, RecommendedLevel, 3)
                .SetNamedEnemyParams(NamedParamId.TargetTitle)
                .SetInfectionType(2),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);

        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdCheckAreaRank(QuestAreaId.MorrowForest, 3);

        // "Get the details of the case from the Area Master and try to solve the problem"
        process0.AddNpcTalkAndOrderBlock(QuestStages.AreaMasterStage, QuestNpcs.AreaMaster, NpcText.AMIntro);

        // Confirm details from <NPC 2608> in <SPOT 878>
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.MorrowForest, QuestNpcs.Requester, NpcText.ReqIntro)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle);

        // Investigate problem locations
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.MainGroup)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle)
            .AddResultCmdQstTalkChg(QuestNpcs.Requester, NpcText.ReqIdle);

        // Defeat the demons that are the source of evil
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.MainGroup, false)
            .AddResultCmdSetAnnounce(QuestAnnounceType.Caution);

        // Report to <NPC 2608> in <SPOT 878>
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.MorrowForest, QuestNpcs.Requester, NpcText.ReqReturn)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle);

        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
