/**
 * @brief Morrow Forest Trial: Traces of Tribulation (Morrow Forest AR13)
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60216003;
    public override ushort RecommendedLevel => 75;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;
    public override StageInfo StageInfo => Stage.MorfaulCentrum;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.AreaTrialOrMission;

    private class EnemyGroupId
    {
        public const uint MainGroup = 1;
        public const uint SecondGroup = 10;
    }

    private class NamedParamId
    {
        public const uint Wandering = 1273; // Wandering <name>
        public const uint Trial = 1432; // Trial <name>
    }

    private class NpcText
    {
        public const int AMIntro = 19290;
        public const int AMIdle = 19291;

        public const int ReqIntro = 20998;
        public const int ReqIdle = 20999;
        public const int ReqReturn = 19294;

        public const int SullivanIntro = 19292;
        public const int SullivanIdle = 19293;
        public const int SullivanMid = 20982;
        public const int SullivanReturn = 20983;
        public const int SullivanShout = 20984;
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

    private static class GeneralAnnouncements
    {
        public const int MonsterAppeared = 100053; // "A monster guarding the barrier has appeared!"
        public const int TalismanDestroyed1 = 741; // "One of the talismans that set up the barrier has been destroyed! (2 remaining)"
        public const int TalismanDestroyed2 = 100056; //"One of the talismans that set up the barrier has been destroyed! (1 remaining)"
        public const int BarriedLifted = 806; // "The seal on the barrier has been lifted! -- NPC rescued successfully!"
    }

    private static class Flags
    {
        // Control
        public const int ControlSpawnTalismans = 1;
        public const int ControlTalismansBroken = 2;

        // MyQst
        // Like with q60214003, the JP comments are all wrong.
        public const int Fsm0 = 2068;	// 束縛時;  Starts his first combat routine
        public const int Fsm1 = 2495;	// 戦闘開始; Waits for dialog in the first area
        public const int Fsm2 = 2508;	// 待機; Starts his second combat routine
        public const int Fsm3 = 2510;	// 戦闘開始; Waits for dialog in the second area


        // Layout
        public const int OmDestroy1 = 4306; // 破壊OM１; Destroy OM1, (0, 0)
        public const int OmDestroy2 = 4307; // 破壊OM２; Destroy OM2, (1, 0)
        public const int OmDestroy3 = 4533; // 破壊OM３; Destroy OM3, (2, 0)
        public const int AlliedNPC = 4989; // 共闘NPC; Allied NPC (3, 0)
        public const int OmBarrier = 4988; // 結界OM; Barrier OM, (4, 0)
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
        AddPointReward(PointType.ExperiencePoints, 27350);
        AddWalletReward(WalletType.Gold, 8987);
        AddWalletReward(WalletType.RiftPoints, 1330);

        AddFixedItemReward(ItemId.MorrowLauanWood, 5);

    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.MainGroup, QuestStages.TargetStage, 20, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.StoutUndead, RecommendedLevel, 0)
                .SetNamedEnemyParams(NamedParamId.Trial),
            LibDdon.Enemy.CreateAuto(EnemyId.StoutUndead, RecommendedLevel, 1)
                .SetNamedEnemyParams(NamedParamId.Trial),
            LibDdon.Enemy.CreateAuto(EnemyId.StoutUndead, RecommendedLevel, 2)
                .SetNamedEnemyParams(NamedParamId.Trial),
            LibDdon.Enemy.CreateAuto(EnemyId.DarkCorpsePunisher, RecommendedLevel, 3)
                .SetNamedEnemyParams(NamedParamId.Trial),
            LibDdon.Enemy.CreateAuto(EnemyId.DarkCorpsePunisher, RecommendedLevel, 4)
                .SetNamedEnemyParams(NamedParamId.Trial),
            LibDdon.Enemy.CreateAuto(EnemyId.DarkCorpsePunisher, RecommendedLevel, 5)
                .SetNamedEnemyParams(NamedParamId.Trial)
        });

        AddEnemies(EnemyGroupId.SecondGroup, QuestStages.TargetStage, 21, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Medusa, RecommendedLevel, 0, isBoss:true)
                .SetNamedEnemyParams(NamedParamId.Wandering),
            LibDdon.Enemy.CreateAuto(EnemyId.PhindymianWarrior, RecommendedLevel, 1)
                .SetNamedEnemyParams(NamedParamId.Wandering),
            LibDdon.Enemy.CreateAuto(EnemyId.PhindymianDefender, RecommendedLevel, 2)
                .SetNamedEnemyParams(NamedParamId.Wandering),
            LibDdon.Enemy.CreateAuto(EnemyId.PhindymianDefender, RecommendedLevel, 3)
                .SetNamedEnemyParams(NamedParamId.Wandering),
            LibDdon.Enemy.CreateAuto(EnemyId.PhindymianPriest, RecommendedLevel, 4)
                .SetNamedEnemyParams(NamedParamId.Wandering),
            LibDdon.Enemy.CreateAuto(EnemyId.PhindymianPriest, RecommendedLevel, 5)
                .SetNamedEnemyParams(NamedParamId.Wandering),
        });

        // Dummy group to clear the spawn.
        AddEnemies(EnemyGroupId.MainGroup+1, QuestStages.TargetStage, 20, QuestEnemyPlacementType.Automatic, new());

        // Dummy group to clear the spawn.
        AddEnemies(EnemyGroupId.SecondGroup+1, QuestStages.TargetStage, 40, QuestEnemyPlacementType.Automatic, new());
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);

        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdCheckAreaRank(QuestAreaId.MorrowForest, 12);

        // 1	"Get the details of the case from the Area Master and try to solve the problem"
        process0.AddNpcTalkAndOrderBlock(QuestStages.AreaMasterStage, QuestNpcs.AreaMaster, NpcText.AMIntro);

        // 2	Confirm details from <NPC 2604>
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.MorrowForest, QuestNpcs.Requester, NpcText.ReqIntro)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle);

        // 3	"Search for <NPC 561> near where you headed to investigate"
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, QuestStages.TargetStage, 3, 0, NpcId.Sullivan, NpcText.SullivanIntro)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle)
            .AddResultCmdQstTalkChg(QuestNpcs.Requester, NpcText.ReqIdle)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, Flags.AlliedNPC, preventReplay:true)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, Flags.OmBarrier, preventReplay:true)
            // .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, Flags.FsmWait)
            .AddEnemyGroupId(EnemyGroupId.MainGroup + 1) // Ensure the ambush is clear
        ;

        // 4	Defeat the demons restraining the comrades
        process0.AddSpawnGroupBlock(QuestAnnounceType.Update, EnemyGroupId.MainGroup, true)
            .AddResultCmdQstTalkChg(NpcId.Sullivan, NpcText.SullivanIdle)
            .AddResultCmdPlayMessage(NpcText.SullivanIdle, 0)
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

        // Free Sullivan and start his combat routine
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, Flags.OmBarrier)
            .AddCheckCmdDieEnemy(QuestStages.TargetStage, 20, -1)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, Flags.Fsm0)
        ;

        // 5    "Inquire with <NPC 561> about where the captured monsters are."
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.MorrowForest, 3, 0, NpcId.Sullivan, NpcText.SullivanMid)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, Flags.Fsm1)
        ;

        // 6    "Head to the location of the monster caught in the trap"
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.SecondGroup)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, Flags.Fsm2)
            .AddEnemyGroupId(EnemyGroupId.SecondGroup+1) // Clear adjacent group.
        ;

        // 7    Defeat the demons
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.SecondGroup, false);

        // Can't have a PlayMessage on a checkpoint block or it'll play on login.
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.MorrowForest, false)
            .AddResultCmdPlayMessage(NpcText.SullivanShout, 0); 

        // 8	Report to <NPC 2604> in <SPOT 878>
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.MorrowForest, QuestNpcs.Requester, NpcText.ReqReturn)
            .AddResultCmdQstTalkChg(NpcId.Sullivan, NpcText.SullivanReturn)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, Flags.Fsm3)
        ;

        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
