/**
 * @brief Kingal Canyon Trial: Proof of Atrocities (Kingal Canyon AR7)
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60217001;
    public override ushort RecommendedLevel => 73;
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
        public const uint TargetTitle = 1254; // Life Slaughtering <name>
    }

    private class NpcText
    {
        public const int AMIntro = 19270;
        public const int AMIdle = 19271;

        public const int ReqIntro = 19272;
        public const int ReqIdle = 19273;
        public const int ReqReturn = 19274;
    }

    private class QuestNpcs
    {
        public const NpcId AreaMaster = NpcId.Ciaran;
        public const NpcId Requester = NpcId.Boadicea;
    }

    private class QuestStages 
    {
        public static readonly StageInfo AreaMasterStage = Stage.GlyndwrCentrum;
        public static readonly StageInfo TargetStage = Stage.KingalCanyon;
    }

    private class Flags {
        // MyQst Control
        public const uint ControlSpawnOm = 1;
        public const uint ControlOm1Collected = 2;
        public const uint ControlOm2Collected = 3;

        // Layout
        public const uint OmSearch1 = 4296; // 調べる１, (0, 0)
        public const uint OmSearch2 = 4297; // 調べる２, (1, 0)
    }

    private static class GeneralAnnouncements
    {
        public const int OmObtained1 = 737; 
        public const int OmObtained2 = 779;
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 21850);
        AddWalletReward(WalletType.Gold, 8816);
        AddWalletReward(WalletType.RiftPoints, 1110);

        AddFixedItemReward(ItemId.ValleySpringWater, 10);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.MainGroup, QuestStages.TargetStage, 18, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Grigori, RecommendedLevel, 0)
                .SetNamedEnemyParams(NamedParamId.TargetTitle),
            LibDdon.Enemy.CreateAuto(EnemyId.Grigori, RecommendedLevel, 1)
                .SetNamedEnemyParams(NamedParamId.TargetTitle),
            LibDdon.Enemy.CreateAuto(EnemyId.Grigori, RecommendedLevel, 2)
                .SetNamedEnemyParams(NamedParamId.TargetTitle),
            LibDdon.Enemy.CreateAuto(EnemyId.Grigori, RecommendedLevel, 3)
                .SetNamedEnemyParams(NamedParamId.TargetTitle),
            LibDdon.Enemy.CreateAuto(EnemyId.Grigori, RecommendedLevel, 4)
                .SetNamedEnemyParams(NamedParamId.TargetTitle)
        });
    }

    private class InstanceData
    {
        public static void SetOmCount(QuestState questState, int value)
        {
            questState.InstanceVars.SetData<int>("om_count", value);
        }

        public static int GetOmCount(QuestState questState)
        {
            return questState.InstanceVars.GetData<int>("om_count");
        }
    }

    public override void InitializeInstanceState(QuestState questState)
    {
        InstanceData.SetOmCount(questState, 0);
    }

    private void UpdateOmCb(QuestCallbackParam param)
    {
        var count = InstanceData.GetOmCount(param.QuestState);
        
        if (count == 0)
        {
            InstanceData.SetOmCount(param.QuestState, 1);
            param.ResultCommands.AddResultCmdMyQstFlagOn(Flags.ControlOm1Collected);
            return;
        }
        else if (count == 1)
        {
            InstanceData.SetOmCount(param.QuestState, 2);
            param.ResultCommands.AddResultCmdMyQstFlagOn(Flags.ControlOm2Collected);
            return;
        }
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);

        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdCheckAreaRank(QuestAreaId.KingalCanyon, 6);

        // 1	"Get the details of the case from the Area Master and try to solve the problem"
        process0.AddNpcTalkAndOrderBlock(QuestStages.AreaMasterStage, QuestNpcs.AreaMaster, NpcText.AMIntro);

        // 2	Confirm with <NPC 2707> in <SPOT 879>
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, QuestStages.TargetStage, QuestNpcs.Requester, NpcText.ReqIntro)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle);

        // 3	Investigate the incident
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.MainGroup, true)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle)
            .AddResultCmdQstTalkChg(QuestNpcs.Requester, NpcText.ReqIdle);

        // 4	Defeat the demons
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.MainGroup, false);

        // 5	Investigate traces of fiends (2 remaining)
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, Flags.ControlSpawnOm)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.CheckOn, Flags.ControlOm1Collected);

        // 6	Investigate traces of fiends (1 remaining)
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.CheckOn, Flags.ControlOm2Collected);

        // 7	Report to <NPC 2707> in <SPOT 879>
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, QuestStages.TargetStage, QuestNpcs.Requester, NpcText.ReqReturn)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle);

        process0.AddProcessEndBlock(true);


        // Om process A
        var process1 = AddNewProcess(1);

        process1.AddRawBlock(QuestAnnounceType.None)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.CheckOn, Flags.ControlSpawnOm);

        process1.AddRawBlock(QuestAnnounceType.None)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, Flags.OmSearch1)
            .AddCheckCmdQuestOmReleaseTouch(QuestStages.TargetStage, 0, 0, true);
        
        process1.AddRawBlock(QuestAnnounceType.None)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, Flags.OmSearch1)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.OmObtained2, true)
            .AddCallback(UpdateOmCb);

        // Om process B
        var process2 = AddNewProcess(2);

        process2.AddRawBlock(QuestAnnounceType.None)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.CheckOn, Flags.ControlSpawnOm);

        process2.AddRawBlock(QuestAnnounceType.None)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, Flags.OmSearch2)
            .AddCheckCmdQuestOmReleaseTouch(QuestStages.TargetStage, 1, 0, true);
        
        process2.AddRawBlock(QuestAnnounceType.None)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, Flags.OmSearch2)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.OmObtained2, true)
            .AddCallback(UpdateOmCb);

    }
}

return new ScriptedQuest();
