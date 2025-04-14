/**
 * @brief Morrow Forest Trial: Flight of Sorrow (Morrow Forest AR15)
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60216004;
    public override ushort RecommendedLevel => 80;
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
        public const uint TargetTitle = 1274; // Corruption Spreading Eagle
    }

    private class TargetEnemyId
    {
        public const EnemyId Target = EnemyId.SeverelyInfectedGriffin;
        
    }

    private class NpcText
    {
        public const int AMIntro = 19350;
        public const int AMIdle = 19351;

        public const int ReqIntro = 19352;
        public const int ReqIdle = 19353;
        public const int ReqReturn = 19354;
    }

    private class QuestNpcs
    {
        public const NpcId AreaMaster = NpcId.Arthfael;
        public const NpcId Requester = NpcId.Thorin;
    }

    private class QuestStages 
    {
        public static readonly StageInfo AreaMasterStage = Stage.MorfaulCentrum;
        public static readonly StageInfo TargetStage = Stage.MorrowForest;
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 40350);
        AddWalletReward(WalletType.Gold, 10277);
        AddWalletReward(WalletType.RiftPoints, 1880);

        AddFixedItemReward(ItemId.MorrowMedal, 1);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.MainGroup, QuestStages.TargetStage, 31, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(TargetEnemyId.Target, RecommendedLevel, 0, isBoss:true)
                .SetNamedEnemyParams(NamedParamId.TargetTitle)
                .SetInfectionType(3), // Critical, 危険,
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);

        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdCheckAreaRank(QuestAreaId.MorrowForest, 14);

        // "Get the details of the case from the Area Master and try to solve the problem"
        process0.AddNpcTalkAndOrderBlock(QuestStages.AreaMasterStage, QuestNpcs.AreaMaster, NpcText.AMIntro);

        // Confirm details from <NPC 2605> in <SPOT 878>
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.MorrowForest, QuestNpcs.Requester, NpcText.ReqIntro)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle);

        // Investigate problem locations
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.MainGroup)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle)
            .AddResultCmdQstTalkChg(QuestNpcs.Requester, NpcText.ReqIdle);

        // Defeat the demons that are the source of evil
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.MainGroup, false)
            .AddResultCmdSetAnnounce(QuestAnnounceType.Caution);

        // Report to <NPC 2605> in <SPOT 878>
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.MorrowForest, QuestNpcs.Requester, NpcText.ReqReturn)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle);

        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
