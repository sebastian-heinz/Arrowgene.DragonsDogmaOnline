/**
 * @brief Kingal Canyon Trial: A Desired Transformation (Kingal Canyon AR15)
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60217004;
    public override ushort RecommendedLevel => 80;
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
        public const uint TargetTitle = 1276; // Disaster Bringer
        public const uint Trial = 1432; // Trial <name>
    }

    private class NpcText
    {
        public const int AMIntro = 19355;
        public const int AMIdle = 19356;

        public const int ReqIntro = 19357;
        public const int ReqIdle = 19358;

        public const int AMReturn = 19359;
    }

    private class QuestNpcs
    {
        public const NpcId AreaMaster = NpcId.Ciaran;
        public const NpcId Requester = NpcId.Adelin;
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

        AddFixedItemReward(ItemId.KingalMedal, 1);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.MainGroup, Stage.AncientUndergroundShrine, 3, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.SeverelyInfectedScourge, RecommendedLevel, 0, isBoss:true)
                .SetNamedEnemyParams(NamedParamId.TargetTitle)
                .SetInfectionType(1), // Mild
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedDirewolf, RecommendedLevel, 5)
                .SetNamedEnemyParams(NamedParamId.Trial),
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedDirewolf, RecommendedLevel, 14)
                .SetNamedEnemyParams(NamedParamId.Trial),
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedSnowHarpy, RecommendedLevel, 10)
                .SetNamedEnemyParams(NamedParamId.Trial),
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedSnowHarpy, RecommendedLevel, 11)
                .SetNamedEnemyParams(NamedParamId.Trial)
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);

        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdCheckAreaRank(QuestAreaId.KingalCanyon, 14);

        // "Get the details of the case from the Area Master and try to solve the problem"
        process0.AddNpcTalkAndOrderBlock(QuestStages.AreaMasterStage, QuestNpcs.AreaMaster, NpcText.AMIntro);

        // Confirm details from <NPC 2708> in <SPOT 879>
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, QuestStages.TargetStage, QuestNpcs.Requester, NpcText.ReqIntro)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle);

        // Investigate problem locations
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.MainGroup, true)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle)
            .AddResultCmdQstTalkChg(QuestNpcs.Requester, NpcText.ReqIdle);

        // Defeat the demons that are the source of evil
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.MainGroup, false)
            .AddResultCmdSetAnnounce(QuestAnnounceType.Caution);

        // Report to <NPC 2700> in <SPOT 879>
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, QuestStages.AreaMasterStage, QuestNpcs.AreaMaster, NpcText.AMReturn);

        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
