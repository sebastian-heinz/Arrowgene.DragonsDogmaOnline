/**
 * @brief Kingal Canyon Trial: Behind the Closed Doors (Kingal Canyon AR10)
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60217002;
    public override ushort RecommendedLevel => 75;
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
        public const uint TargetTitle = 1255; // Nesting in the Temple
        public const uint Trial = 1432; // Trial <name>
    }

    private class NpcText
    {
        public const int AMIntro = 19335;
        public const int AMIdle = 19336;

        public const int ReqIntro = 19337;
        public const int ReqIdle = 19338;
        public const int ReqReturn = 19339;
    }

    private class QuestNpcs
    {
        public const NpcId AreaMaster = NpcId.Ciaran;
        public const NpcId Requester = NpcId.Mavail;
    }

    private class QuestStages 
    {
        public static readonly StageInfo AreaMasterStage = Stage.GlyndwrCentrum;
        public static readonly StageInfo TargetStage = Stage.KingalCanyon;
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 24350);
        AddWalletReward(WalletType.Gold, 8987);
        AddWalletReward(WalletType.RiftPoints, 1330);

        AddFixedItemReward(ItemId.Kingalite, 10);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.MainGroup, Stage.ShadoleanGreatTemple2, 7, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Behemoth0, RecommendedLevel, 0, isBoss:true)
                .SetNamedEnemyParams(NamedParamId.TargetTitle),
            LibDdon.Enemy.CreateAuto(EnemyId.Gargoyle, RecommendedLevel, 4)
                .SetNamedEnemyParams(NamedParamId.Trial),
            LibDdon.Enemy.CreateAuto(EnemyId.Gargoyle, RecommendedLevel, 5)
                .SetNamedEnemyParams(NamedParamId.Trial),
            LibDdon.Enemy.CreateAuto(EnemyId.Grigori, RecommendedLevel, 2)
                .SetNamedEnemyParams(NamedParamId.Trial),
            LibDdon.Enemy.CreateAuto(EnemyId.Grigori, RecommendedLevel, 12)
                .SetNamedEnemyParams(NamedParamId.Trial)
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);

        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdCheckAreaRank(QuestAreaId.KingalCanyon, 9);

        // "Get the details of the case from the Area Master and try to solve the problem"
        process0.AddNpcTalkAndOrderBlock(QuestStages.AreaMasterStage, QuestNpcs.AreaMaster, NpcText.AMIntro);

        // Confirm details from <NPC 2706> in <SPOT 879>
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, QuestStages.TargetStage, QuestNpcs.Requester, NpcText.ReqIntro)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle);

        // Investigate problem locations
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.MainGroup, true)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle)
            .AddResultCmdQstTalkChg(QuestNpcs.Requester, NpcText.ReqIdle);

        // Defeat the demons that are the source of evil
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.MainGroup, false)
            .AddResultCmdSetAnnounce(QuestAnnounceType.Caution);

        // Report to <NPC 2706> in <SPOT 879>
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, QuestStages.TargetStage, QuestNpcs.Requester, NpcText.ReqReturn)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle);

        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
