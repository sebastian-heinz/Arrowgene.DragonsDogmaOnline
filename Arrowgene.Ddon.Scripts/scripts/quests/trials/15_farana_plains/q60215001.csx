/**
 * @brief Farana Plains Trial: The Honor Guards (Farana Plains AR7)
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60215001;
    public override ushort RecommendedLevel => 73;
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
        public const uint TargetTitle = 1245; // Grave Keeper who stole the key
        public const uint TrialTitle = 1432; // Trial
    }

    private class NpcText
    {
        public const int AMIntro = 19260;
        public const int AMIdle = 19261;

        public const int ReqIntro = 19262;
        public const int ReqIdle = 19263;
        public const int ReqReturn = 19264;
    }

    private class QuestNpcs
    {
        public const NpcId AreaMaster = NpcId.Razanailt;
        public const NpcId Requester = NpcId.Wine;
    }

    private class QuestStages 
    {
        public static readonly StageInfo AreaMasterStage = Stage.DanaCentrum;
        public static readonly StageInfo TargetStage = Stage.FaranaPlains0;
    }

    private class Flags {
        public const uint ControlSpawnKeys = 1;
        public const uint ControlKey1Collected = 2;
        public const uint ControlKey2Collected = 3;

        public const uint OmSearch1 = 4290; // 調べる１, (0, 0)
        public const uint OmSearch2 = 4291; // 調べる２, (1, 0)
    }

    private static class GeneralAnnouncements
    {
        public const int BrokenKeyFragment1 = 735; // "Obtained a <Broken Key Fragment>"
        public const int BrokenKeyFragment2 = 781; // "Obtained a <Broken Key Fragment>"
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 21850);
        AddWalletReward(WalletType.Gold, 8816);
        AddWalletReward(WalletType.RiftPoints, 1110);

        AddFixedItemReward(ItemId.SparklingWater, 10);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.MainGroup, QuestStages.TargetStage, 50, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Mudman, RecommendedLevel, 10)
                .SetNamedEnemyParams(NamedParamId.TrialTitle),
            LibDdon.Enemy.CreateAuto(EnemyId.Mudman, RecommendedLevel, 9)
                .SetNamedEnemyParams(NamedParamId.TrialTitle),
            LibDdon.Enemy.CreateAuto(EnemyId.Mudman, RecommendedLevel, 8)
                .SetNamedEnemyParams(NamedParamId.TrialTitle),
            LibDdon.Enemy.CreateAuto(EnemyId.GhostMail, RecommendedLevel, 7)
                .SetNamedEnemyParams(NamedParamId.TargetTitle),
            LibDdon.Enemy.CreateAuto(EnemyId.GhostMail, RecommendedLevel, 6)
                .SetNamedEnemyParams(NamedParamId.TargetTitle)
        });
    }

    private class InstanceData
    {
        public static void SetKeyCount(QuestState questState, int value)
        {
            questState.InstanceVars.SetData<int>("key_count", value);
        }

        public static int GetKeyCount(QuestState questState)
        {
            return questState.InstanceVars.GetData<int>("key_count");
        }
    }

    public override void InitializeInstanceState(QuestState questState)
    {
        InstanceData.SetKeyCount(questState, 0);
    }

    private void UpdateKeysCb(QuestCallbackParam param)
    {
        var count = InstanceData.GetKeyCount(param.QuestState);
        
        if (count == 0)
        {
            InstanceData.SetKeyCount(param.QuestState, 1);
            param.ResultCommands.AddResultCmdMyQstFlagOn(Flags.ControlKey1Collected);
            return;
        }
        else if (count == 1)
        {
            InstanceData.SetKeyCount(param.QuestState, 2);
            param.ResultCommands.AddResultCmdMyQstFlagOn(Flags.ControlKey2Collected);
            return;
        }
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);

        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdCheckAreaRank(QuestAreaId.FaranaPlains, 6);

        // 1	"Get the details of the case from the Area Master and try to solve the problem"
        process0.AddNpcTalkAndOrderBlock(QuestStages.AreaMasterStage, QuestNpcs.AreaMaster, NpcText.AMIntro);

        // 2	Confirm with <NPC 2508> in <SPOT 877>
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, QuestStages.TargetStage, QuestNpcs.Requester, NpcText.ReqIntro)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle);

        // 3	Investigate the incident
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.MainGroup)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle)
            .AddResultCmdQstTalkChg(QuestNpcs.Requester, NpcText.ReqIdle);

        // 4	Defeat the demons
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.MainGroup, false);

        // 5	Look for dropped keys.
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, Flags.ControlSpawnKeys)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.CheckOn, Flags.ControlKey1Collected);

        // 6	Find the other piece of the broken key
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.CheckOn, Flags.ControlKey2Collected);

        // 7	Report to <NPC 2508> in <SPOT 877>
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, QuestStages.TargetStage, QuestNpcs.Requester, NpcText.ReqReturn)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle);

        process0.AddProcessEndBlock(true);


        // Key process A
        var process1 = AddNewProcess(1);

        process1.AddRawBlock(QuestAnnounceType.None)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.CheckOn, Flags.ControlSpawnKeys);

        process1.AddRawBlock(QuestAnnounceType.None)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, Flags.OmSearch1)
            .AddCheckCmdQuestOmReleaseTouch(QuestStages.TargetStage, 0, 0, true);
        
        process1.AddRawBlock(QuestAnnounceType.None)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, Flags.OmSearch1)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.BrokenKeyFragment2, true)
            .AddCallback(UpdateKeysCb);

        // Key process B
        var process2 = AddNewProcess(2);

        process2.AddRawBlock(QuestAnnounceType.None)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.CheckOn, Flags.ControlSpawnKeys);

        process2.AddRawBlock(QuestAnnounceType.None)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, Flags.OmSearch2)
            .AddCheckCmdQuestOmReleaseTouch(QuestStages.TargetStage, 1, 0, true);
        
        process2.AddRawBlock(QuestAnnounceType.None)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, Flags.OmSearch2)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.BrokenKeyFragment2, true)
            .AddCallback(UpdateKeysCb);

    }
}

return new ScriptedQuest();
