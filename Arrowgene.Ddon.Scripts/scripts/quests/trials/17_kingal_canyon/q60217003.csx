/**
 * @brief Kingal Canyon Trial: Memories of Desertion (Kingal Canyon AR13)
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60217003;
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
        public const uint Forgotten = 1275; // Forgotten <name>
        public const uint Trial = 1432; // Trial <name>
    }

    private class NpcText
    {
        public const int AMIntro = 19295;
        public const int AMIdle = 19296;

        public const int ReqIntro = 20989;
        public const int ReqIdle = 20990;
        public const int ReqReturn = 12999;

        public const int IdeIntro = 19297;
        public const int IdeIdle = 19298;

        public const int IdeReturn = 20991;
        public const int IdeShout = 20992;
    }

    private class QuestNpcs
    {
        public const NpcId AreaMaster = NpcId.Ciaran;
        public const NpcId Requester = NpcId.Adelin;
    }

    private class QuestStages 
    {
        public static readonly StageInfo AreaMasterStage = Stage.GlyndwrCentrum;
        public static readonly StageInfo TargetStage = Stage.AmburosGreatRuins;
    }

    private static class GeneralAnnouncements
    {
        public const int MonsterAppeared = 100054; // "A monster guarding the barrier has appeared!"
        public const int TalismanDestroyed1 = 742; // "One of the talismans that set up the barrier has been destroyed! (2 remaining)"
        public const int TalismanDestroyed2 = 100058; //"One of the talismans that set up the barrier has been destroyed! (1 remaining)"
        public const int BarriedLifted = 807; // "The seal on the barrier has been lifted! -- NPC rescued successfully!"
    }

    private static class Flags
    {
        // Control
        public const int ControlSpawnTalismans = 1;
        public const int ControlTalismansBroken = 2;

        // MyQst
        public const int Fsm0 = 2069;	// 束縛時;  
        public const int Fsm1 = 2496;	// 戦闘開始; 
        public const int Fsm2 = 2070;	// 束縛時; 


        // Layout
        public const int OmDestroy1 = 4309; // 破壊OM１; Destroy OM1, (0, 0)
        public const int OmDestroy2 = 4310; // 破壊OM２; Destroy OM2, (1, 0)
        public const int OmDestroy3 = 4534; // 破壊OM３; Destroy OM3, (2, 0)
        public const int AlliedNPC = 4994; // 共闘NPC; Allied NPC (3, 0)
        public const int OmBarrier = 4993; // 結界OM; Barrier OM, (4, 0)
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

        AddFixedItemReward(ItemId.AdamasOre, 5);

    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.MainGroup, QuestStages.TargetStage, 3, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.GigantMachina, RecommendedLevel, 9, isBoss:true)
                .SetNamedEnemyParams(NamedParamId.Forgotten),
            LibDdon.Enemy.CreateAuto(EnemyId.Witch, RecommendedLevel, 10)
                .SetNamedEnemyParams(NamedParamId.Trial),
            LibDdon.Enemy.CreateAuto(EnemyId.Witch, RecommendedLevel, 11)
                .SetNamedEnemyParams(NamedParamId.Trial)
        });

        // Dummy group to clear the spawn.
        AddEnemies(EnemyGroupId.MainGroup+1, QuestStages.TargetStage, 3, QuestEnemyPlacementType.Automatic, new());
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);

        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdCheckAreaRank(QuestAreaId.KingalCanyon, 12);

        // 1	"Get the details of the case from the Area Master and try to solve the problem"
        process0.AddNpcTalkAndOrderBlock(QuestStages.AreaMasterStage, QuestNpcs.AreaMaster, NpcText.AMIntro);

        // 2	Confirm details from <NPC 2708>
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.KingalCanyon, QuestNpcs.Requester, NpcText.ReqIntro)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle);

        // 3	"Head to <STG 863> and search for <NPC 4758>"
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, QuestStages.TargetStage, 3, 0, NpcId.Ide, NpcText.IdeIntro)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle)
            .AddResultCmdQstTalkChg(QuestNpcs.Requester, NpcText.ReqIdle)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, Flags.AlliedNPC, preventReplay:true)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, Flags.OmBarrier, preventReplay:true)
            .AddEnemyGroupId(EnemyGroupId.MainGroup + 1) // Ensure the ambush is clear
        ;

        // 4	Defeat the demons restraining the comrades
        process0.AddSpawnGroupBlock(QuestAnnounceType.Update, EnemyGroupId.MainGroup, true)
            .AddResultCmdPlayMessage(NpcText.IdeIdle, 0)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.MonsterAppeared, true)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, Flags.ControlSpawnTalismans, preventReplay:true)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.CheckOn, Flags.ControlTalismansBroken)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, Flags.Fsm0)
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

        // Free Ide and start her combat routine
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, Flags.OmBarrier)
            .AddCheckCmdDieEnemy(QuestStages.TargetStage, 3, -1)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, Flags.Fsm2)
        ;

        // Can't have a PlayMessage on a checkpoint block or it'll play on login.
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.AmburosGreatRuins, false)
            .AddResultCmdPlayMessage(NpcText.IdeShout, 0); 

        // 8	Report to <NPC 2604> in <SPOT 878>
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.KingalCanyon, QuestNpcs.Requester, NpcText.ReqReturn)
            .AddResultCmdQstTalkChg(NpcId.Ide, NpcText.IdeReturn)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, Flags.Fsm1)
        ;

        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
