/**
 * @brief Farana Plains Trial: Approaching Echoes of Doom (Farana Plains AR10)
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60215002;
    public override ushort RecommendedLevel => 75;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;
    public override StageInfo StageInfo => Stage.DanaCentrum;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.AreaTrialOrMission;

    private class EnemyGroupId
    {
        public const uint MainGroup = 1;
    }

    private class NamedParamId
    {
        public const uint TargetTitle = 1246; // Rumbling Shell Dragon
    }

    private class TargetEnemyId
    {
        public const EnemyId Target = EnemyId.Tarasque;
    }

    private class NpcText
    {
        public const int AMIntro = 19305;
        public const int AMIdle = 19306;

        public const int ReqIntro = 19307;
        public const int ReqIdle = 19308;
        public const int ReqReturn = 19309;
    }

    private class QuestNpcs
    {
        public const NpcId AreaMaster = NpcId.Razanailt;
        public const NpcId Requester = NpcId.Fionn;
    }

    private class QuestStages 
    {
        public static readonly StageInfo AreaMasterStage = Stage.DanaCentrum;
        public static readonly StageInfo TargetStage = Stage.FaranaPlains0;
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 24350);
        AddWalletReward(WalletType.Gold, 8987);
        AddWalletReward(WalletType.RiftPoints, 1330);

        AddFixedItemReward(ItemId.FaranaMedal, 1);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.MainGroup, QuestStages.TargetStage, 24, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(TargetEnemyId.Target, RecommendedLevel, 6, isBoss:true)
                .SetNamedEnemyParams(NamedParamId.TargetTitle)
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);

        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdCheckAreaRank(QuestAreaId.FaranaPlains, 9);

        // "Get the details of the case from the Area Master and try to solve the problem"
        process0.AddNpcTalkAndOrderBlock(QuestStages.AreaMasterStage, QuestNpcs.AreaMaster, NpcText.AMIntro);

        // Confirm details from <NPC 2507> in <SPOT 877>
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, QuestStages.TargetStage, QuestNpcs.Requester, NpcText.ReqIntro)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle);

        // Investigate problem locations
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.MainGroup)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle)
            .AddResultCmdQstTalkChg(QuestNpcs.Requester, NpcText.ReqIdle);

        // Defeat the demons that are the source of evil
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.MainGroup, false)
            .AddResultCmdSetAnnounce(QuestAnnounceType.Caution);

        // Report to <NPC 2507> in <SPOT 877>
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, QuestStages.TargetStage, QuestNpcs.Requester, NpcText.ReqReturn)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle);

        process0.AddProcessEndBlock(true);

    }
}

return new ScriptedQuest();
