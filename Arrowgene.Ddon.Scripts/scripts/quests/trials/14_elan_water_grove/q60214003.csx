/**
 * @brief Elan Water Grove Trial: Green Delusions (Elan Water Grove AR13)
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60214003;
    public override ushort RecommendedLevel => 75;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;
    public override StageInfo StageInfo => Stage.ProtectorsRetreat;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.AreaTrialOrMission;

    private class EnemyGroupId
    {
        public const uint GlenisGroup = 0;
        public const uint SecondGroup = 10;
    }

    private class NamedParamId
    {
        public const uint TargetTitle = 1432; // Trial <name>
    }

    private class NpcText
    {
        public const int MuselIntro = 19285;
        public const int MuselIdle = 19286;


        public const int DirithIntro = 20996;
        public const int DirithIdle = 20997;

        public const int GlenisIntro = 19287;
        public const int GlenisIdle = 19288;
        public const int GlenisMid = 20971;
        public const int GlenisReturn = 20972;
        public const int GlenisShout = 20995;

        public const int DirithReturn = 19289;
    }

    private static class GeneralAnnouncements
    {
        public const int MonsterAppeared = 100047; // "A monster guarding the barrier has appeared!"
        public const int TalismanDestroyed1 = 740; // "One of the talismans that set up the barrier has been destroyed! (2 remaining)"
        public const int TalismanDestroyed2 = 100055; //"One of the talismans that set up the barrier has been destroyed! (1 remaining)"
        public const int BarriedLifted = 804; // "The seal on the barrier has been lifted! -- NPC rescued successfully!"
    }

    private class Flags
    {
        // Control
        public const int ControlSpawnTalismans = 1;
        public const int ControlTalismansBroken = 2;

        // MyQst; the comments on these in the FSM json are totally wrong and flipped around.
        public const uint Fsm0 = 2066; // 束縛時; "When Bound", actually triggers her first combat routine.
        public const uint Fsm1 = 2494; // 戦闘開始; "Start Battle", actually makes her crouch and enables dialog.
        public const uint Fsm2 = 2503; // 待機; "Wait", actually triggers her second combat routine.
        public const uint Fsm3 = 2505; // 戦闘開始; "Start Battle", actually makes her stand still and enables dialog.

        // Layout
        public const uint OmDestroy1 = 4303; // 破壊OM１; Destroy OM1, (0, 0)
        public const uint OmDestroy2 = 4304; // 破壊OM２; Destroy OM2, (1, 0)
        public const uint OmDestroy3 = 4532; // 破壊OM３; Destroy OM3, (2, 0)
        public const uint OmBarrier = 4985; // 結界OM; Barrier OM, (3, 0)
        public const uint Glenis = 4986; // 共闘NPC; Allied NPC = Glenis, (4, 0)
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

        AddFixedItemReward(ItemId.SmoothWhiteMud, 5);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.GlenisGroup, Stage.ElanWaterGrove, 27, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Wight0, RecommendedLevel, 0)
                .SetNamedEnemyParams(NamedParamId.TargetTitle),
            LibDdon.Enemy.CreateAuto(EnemyId.Wight0, RecommendedLevel, 1)
                .SetNamedEnemyParams(NamedParamId.TargetTitle),
            LibDdon.Enemy.CreateAuto(EnemyId.Ghost, RecommendedLevel, 2)
                .SetNamedEnemyParams(NamedParamId.TargetTitle),
            LibDdon.Enemy.CreateAuto(EnemyId.Ghost, RecommendedLevel, 3)
                .SetNamedEnemyParams(NamedParamId.TargetTitle),
        });

        AddEnemies(EnemyGroupId.GlenisGroup+1, Stage.ElanWaterGrove, 27, QuestEnemyPlacementType.Manual, new());

        AddEnemies(EnemyGroupId.SecondGroup, Stage.ElanWaterGrove, 5, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.GreenGuardian, RecommendedLevel, 0),
            LibDdon.Enemy.CreateAuto(EnemyId.GreenGuardian, RecommendedLevel, 1),
            LibDdon.Enemy.CreateAuto(EnemyId.GreenGuardian, RecommendedLevel, 2),
            LibDdon.Enemy.CreateAuto(EnemyId.GreenGuardian, RecommendedLevel, 3),
            LibDdon.Enemy.CreateAuto(EnemyId.GreenGuardian, RecommendedLevel, 4),
            LibDdon.Enemy.CreateAuto(EnemyId.Ghost, RecommendedLevel, 5)
                .SetNamedEnemyParams(NamedParamId.TargetTitle),
            LibDdon.Enemy.CreateAuto(EnemyId.Ghost, RecommendedLevel, 6)
                .SetNamedEnemyParams(NamedParamId.TargetTitle),
        });

        AddEnemies(EnemyGroupId.SecondGroup+1, Stage.ElanWaterGrove, 40, QuestEnemyPlacementType.Manual, new());
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);

        // Check area rank
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdCheckAreaRank(QuestAreaId.ElanWaterGrove, 12);

        // 1,Get the details of the case from the Area Master and try to solve the problem
        process0.AddNpcTalkAndOrderBlock(Stage.ProtectorsRetreat, NpcId.Musel0, NpcText.MuselIntro);

        // 2,Confirm details from <Dirith1>
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.ProtectorsRetreat, NpcId.Dirith1, NpcText.DirithIntro)
            .AddResultCmdQstTalkChg(NpcId.Musel0, NpcText.MuselIdle);

        // 3,Search for <Glenis> near where you headed to investigate
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.ElanWaterGrove, 4, 0, NpcId.Glenis, NpcText.GlenisIntro)
            .AddResultCmdQstTalkChg(NpcId.Dirith1, NpcText.DirithIdle)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, Flags.Glenis, preventReplay:true)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, Flags.OmBarrier, preventReplay:true)
            .AddEnemyGroupId(EnemyGroupId.GlenisGroup+1)
        ;

        // 4,Defeat the demons restraining the comrades
        process0.AddSpawnGroupBlock(QuestAnnounceType.Update, EnemyGroupId.GlenisGroup, true)
            .AddResultCmdQstTalkChg(NpcId.Glenis, NpcText.GlenisIdle)
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
                .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.CheckOn, Flags.ControlSpawnTalismans)
            ;

            talismanProcess.AddRawBlock(QuestAnnounceType.None)
                .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, TalismanSpawnFlags[i], preventReplay:true)
                .AddCheckCmdIsOmBrokenQuest(Stage.ElanWaterGrove, (uint)i, 0, true)
            ;
            
            talismanProcess.AddRawBlock(QuestAnnounceType.None)
                .AddCallback(UpdateTalismanCb)
            ;
        }

        // Free Glenis and start her combat routine
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.BarriedLifted)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, Flags.OmBarrier)
            .AddCheckCmdDieEnemy(Stage.ElanWaterGrove, 27, -1)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, Flags.Fsm0)
        ;

        // 5,Inquire with <Glenis> about where the captured monsters are.
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.ElanWaterGrove, 4, 0, NpcId.Glenis, NpcText.GlenisMid)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, Flags.Fsm1)
        ;

        // 6,Head to the location of the monster caught in the trap
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.SecondGroup)
            .AddEnemyGroupId(EnemyGroupId.SecondGroup+1)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, Flags.Fsm2)
        ;

        // 7,Defeat the demons
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.SecondGroup, false);

        // Can't have a play-message on a checkpoint block or it'll play on login.
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.ElanWaterGrove, false)
            .AddResultCmdPlayMessage(NpcText.GlenisShout, 0)
        ;

        // TODO: Something about her FSM still isn't right; she should be available for dialogue after killing the final set of enemies.
        // 8,Report to <Dirith1> in <SPOT 876>
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.ProtectorsRetreat, NpcId.Dirith1, NpcText.DirithReturn)
            .AddResultCmdQstTalkChg(NpcId.Glenis, NpcText.GlenisReturn)
        ;

        process0.AddProcessEndBlock(true);  
    }
}

return new ScriptedQuest();
