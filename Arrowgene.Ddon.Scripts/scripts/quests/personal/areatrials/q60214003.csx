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
        public const uint GlenisGroup = 1;
        public const uint SecondGroup = 2;
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
        // MyQst; the comments on these in the FSM json are totally wrong and flipped around.
        public const uint Fsm2066 = 2066; // 束縛時; "When Bound", actually triggers her first combat routine.
        public const uint Fsm2494 = 2494; // 戦闘開始; "Start Battle", actually makes her crouch and enables dialog.
        public const uint Fsm2503 = 2503; // 待機; "Wait", actually triggers her second combat routine.
        public const uint Fsm2505 = 2505; // 戦闘開始; "Start Battle", actually makes her stand still and enables dialog.

        // Layout
        public const uint OmDestroy1 = 4303; // 破壊OM１; Destroy OM1, (0, 0)
        public const uint OmDestroy2 = 4304; // 破壊OM２; Destroy OM2, (1, 0)
        public const uint OmDestroy3 = 4532; // 破壊OM３; Destroy OM3, (2, 0)
        public const uint OmBarrier = 4985; // 結界OM; Barrier OM, (3, 0)
        public const uint Glenis = 4986; // 共闘NPC; Allied NPC = Glenis, (4, 0)
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
    }

    protected override void InitializeBlocks()
    {
        var flag1 = Flags.Fsm2505;
        var flag2 = Flags.Fsm2066;
        var flag3 = Flags.Fsm2494;
        var flag4 = Flags.Fsm2503;

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
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, Flags.Glenis)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, Flags.OmBarrier)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, Flags.Fsm2505);

        // 4,Defeat the demons restraining the comrades
        process0.AddSpawnGroupBlock(QuestAnnounceType.Update, EnemyGroupId.GlenisGroup, true)
            .AddResultCmdQstTalkChg(NpcId.Glenis, NpcText.GlenisIdle)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.MonsterAppeared)
            // Spawn talismans & check for them being broken
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, Flags.OmDestroy1)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, Flags.OmDestroy2)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, Flags.OmDestroy3)
            .AddCheckCmdIsOmBrokenQuest(Stage.ElanWaterGrove, 0, 0, false)
            .AddCheckCmdIsOmBrokenQuest(Stage.ElanWaterGrove, 1, 0, false)
            .AddCheckCmdIsOmBrokenQuest(Stage.ElanWaterGrove, 2, 0, false);
        ;

        // Free Glenis and start her combat routine
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.BarriedLifted)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, Flags.OmBarrier)
            .AddCheckCmdDieEnemy(Stage.ElanWaterGrove, 27, -1)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, Flags.Fsm2066)
        ;

        // 5,Inquire with <Glenis> about where the captured monsters are.
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.ElanWaterGrove, 4, 0, NpcId.Glenis, NpcText.GlenisMid)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, Flags.Fsm2494)
        ;

        // 6,Head to the location of the monster caught in the trap
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Update, EnemyGroupId.SecondGroup)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, Flags.Fsm2503)
        ;

        // 7,Defeat the demons
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.SecondGroup, false);

        // 8,Report to <Dirith1> in <SPOT 876>
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.ProtectorsRetreat, NpcId.Dirith1, NpcText.DirithReturn)
            .AddResultCmdQstTalkChg(NpcId.Glenis, NpcText.GlenisReturn)
            .AddResultCmdPlayMessage(NpcText.GlenisShout, 0)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Clear, Flags.Fsm2505);

        process0.AddProcessEndBlock(true);  
    }
}

return new ScriptedQuest();
