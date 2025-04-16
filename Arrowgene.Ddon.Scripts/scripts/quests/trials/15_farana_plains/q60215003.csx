/**
 * @brief Farana Plains Trial: Another Branch (Farana Plains AR13)
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60215003;
    public override ushort RecommendedLevel => 78;
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
        public const uint Awakened = 1268; // Awakened <name>
        public const uint Trial = 1432; // Trial <name>
    }

    private class NpcText
    {
        public const int AMIntro = 19280;
        public const int AMIdle = 19281;

        public const int ReqIntro = 20993;
        public const int ReqIdle = 20994;
        public const int ReqReturn = 19284;

        public const int CorentinIntro = 19282;
        public const int CorentinIdle = 19283;
        public const int CorentinShout = 20988;
        public const int CorentinReturn = 20987;
    }

    private class QuestNpcs
    {
        public const NpcId AreaMaster = NpcId.Razanailt;
        public const NpcId Requester = NpcId.Henin;
    }

    private class QuestStages 
    {
        public static readonly StageInfo AreaMasterStage = Stage.DanaCentrum;
        public static readonly StageInfo TargetStage = Stage.SacredRiverRuins;
    }

    private static class GeneralAnnouncements
    {
        public const int MonsterAppeared = 100052; // "A monster guarding the barrier has appeared!"
        public const int TalismanDestroyed1 = 739; // "One of the talismans that set up the barrier has been destroyed! (2 remaining)"
        public const int TalismanDestroyed2 = 100057; //"One of the talismans that set up the barrier has been destroyed! (1 remaining)"
        public const int BarriedLifted = 805; // "The seal on the barrier has been lifted! -- NPC rescued successfully!"
    }

    private static class Flags
    {
        // Control
        public const int ControlSpawnTalismans = 1;
        public const int ControlTalismansBroken = 2;

        // MyQst
        // Like with q60214003, the JP comments are all wrong.
        public const int Fsm0 = 2063;	// 束縛時;  Actually triggers their combat routine.
        public const int Fsm1 = 2493;	// 戦闘開始; Actually makes them run to a spot, stand still, and enables dialogue.
        public const int Fsm2 = 2508;	// 待機; Unused?
        public const int Fsm3 = 2064;	// 束縛時; Actually the initial state, enables dialogue.

        // Layout
        public const int OmDestroy1 = 4300; // 破壊OM１; Destroy OM1, (0, 0)
        public const int OmDestroy2 = 4301; // 破壊OM２; Destroy OM2, (1, 0)
        public const int OmDestroy3 = 4531; // 破壊OM３; Destroy OM3, (2, 0)
        public const int AlliedNPC = 4991; // 共闘NPC; Allied NPC (3, 0)
        public const int OmBarrier = 4992; // 結界OM; Barrier OM, (4, 0)
    }

    private class InstanceData
    {
        public static void SetTalismanCount(QuestState questState, int value)
        {
            questState.InstanceVars.SetData<int>("talisman_count", value);
        }

        public static int GetTalismanCount(QuestState questState)
        {
            return questState.InstanceVars.GetData<int>("talisman_count");
        }
    }

    public override void InitializeInstanceState(QuestState questState)
    {
        InstanceData.SetTalismanCount(questState, 0);
    }

    private void UpdateTalismanCb(QuestCallbackParam param)
    {
        var count = InstanceData.GetTalismanCount(param.QuestState);
        
        switch (count)
        {
            case 0:
                InstanceData.SetTalismanCount(param.QuestState, 1);
                param.ResultCommands.AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.TalismanDestroyed1, true);
                return;
            case 1:
                InstanceData.SetTalismanCount(param.QuestState, 2);
                param.ResultCommands.AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.TalismanDestroyed2, true);
                return;
            case 2:
                InstanceData.SetTalismanCount(param.QuestState, 3);
                param.ResultCommands.AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.BarriedLifted, true);
                param.ResultCommands.AddResultCmdMyQstFlagOn(Flags.ControlTalismansBroken);
                return;
            default:
                return;
        }
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 36850);
        AddWalletReward(WalletType.Gold, 10145);
        AddWalletReward(WalletType.RiftPoints, 1660);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.MainGroup, QuestStages.TargetStage, 3, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Gorechimera0, RecommendedLevel, 0, isBoss:true)
                .SetNamedEnemyParams(NamedParamId.Awakened),
            LibDdon.Enemy.CreateAuto(EnemyId.EmpressGhost, RecommendedLevel, 5)
                .SetNamedEnemyParams(NamedParamId.Trial),
            LibDdon.Enemy.CreateAuto(EnemyId.LuxCorpsePunisher, RecommendedLevel, 6),
            LibDdon.Enemy.CreateAuto(EnemyId.LuxCorpsePunisher, RecommendedLevel, 1),
            LibDdon.Enemy.CreateAuto(EnemyId.LuxCorpsePunisher, RecommendedLevel, 4),
        });

        // Dummy group to clear the spawn.
        AddEnemies(EnemyGroupId.MainGroup+1, QuestStages.TargetStage, 3, QuestEnemyPlacementType.Automatic, new());
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);

        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdCheckAreaRank(QuestAreaId.FaranaPlains, 12);

        // 1	"Get the details of the case from the Area Master and try to solve the problem"
        process0.AddNpcTalkAndOrderBlock(QuestStages.AreaMasterStage, QuestNpcs.AreaMaster, NpcText.AMIntro);

        // 2	"Learn more from <NPC  2556> at <STG 810>"
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.ManunVillage, QuestNpcs.Requester, NpcText.ReqIntro)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle);

        // 3	"Head to <STG 861> and search for <NPC 560>"
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, QuestStages.TargetStage, 3, 0, NpcId.Corentin, NpcText.CorentinIntro)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle)
            .AddResultCmdQstTalkChg(QuestNpcs.Requester, NpcText.ReqIdle)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, Flags.AlliedNPC, preventReplay:true)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, Flags.OmBarrier, preventReplay:true)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, Flags.Fsm3)
            .AddEnemyGroupId(EnemyGroupId.MainGroup + 1) // Ensure the room is clear
        ;

        // 4	Defeat the demons restraining the comrades
        process0.AddSpawnGroupBlock(QuestAnnounceType.Update, EnemyGroupId.MainGroup, true)
            .AddResultCmdQstTalkChg(NpcId.Corentin, NpcText.CorentinIdle)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.MonsterAppeared, true)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, Flags.ControlSpawnTalismans, preventReplay:true)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.CheckOn, Flags.ControlTalismansBroken)
        ;

        // Talisman Processes
        List<uint> TalismanSpawnFlags = new() {Flags.OmDestroy1, Flags.OmDestroy2, Flags.OmDestroy3};
        for (int i = 0; i < 3; i++)
        {
            var talismanProcess = AddNewProcess((ushort)(i+1));

            talismanProcess.AddRawBlock(QuestAnnounceType.None)
                .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.CheckOn, Flags.ControlSpawnTalismans);

            talismanProcess.AddRawBlock(QuestAnnounceType.None)
                .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, TalismanSpawnFlags[i])
                .AddCheckCmdIsOmBrokenQuest(QuestStages.TargetStage, (uint)i, 0, true);
            
            talismanProcess.AddRawBlock(QuestAnnounceType.None)
                .AddCallback(UpdateTalismanCb);
        }

        // Free Corentin and start his combat routine
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, Flags.OmBarrier)
            .AddCheckCmdDieEnemy(QuestStages.TargetStage, 3, -1)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, Flags.Fsm0)
        ;

        // Can't have a play-message on a checkpoint block or it'll play on login.
        process0.AddIsStageNoBlock(QuestAnnounceType.None, QuestStages.TargetStage, false)
            .AddResultCmdPlayMessage(NpcText.CorentinShout, 0) 
        ;

        // 5	Report to <NPC  2556> in <STG 810>
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.ManunVillage, QuestNpcs.Requester, NpcText.ReqReturn)
            .AddResultCmdQstTalkChg(NpcId.Corentin, NpcText.CorentinReturn)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, Flags.Fsm1)
        ;

        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
