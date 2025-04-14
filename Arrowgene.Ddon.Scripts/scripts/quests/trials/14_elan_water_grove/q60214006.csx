/**
 * @brief Elan Water Grove Trial: Tidings of Turmoil (Elan Water Grove AR6)
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60214006;
    public override ushort RecommendedLevel => 70;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;
    public override StageInfo StageInfo => Stage.ProtectorsRetreat;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.AreaTrialOrMission;

    private class MyQstFlag
    {
        public const uint FsmFlag = 2486; // ???
        public const uint Ide = 4959;
        public const uint Nora = 4960;
        public const uint Glenis = 4961;

        public const uint GriffinSpawned = 1;
    }

    private class EnemyGroupId
    {
        public const uint InitialGroup = 1;
        public const uint AmbushGroup = 10;
    }

    private class NamedParamId
    {
        public const uint GriffinTitle = 1467; // "???"
    }

    private class GeneralAnnouncements
    {
        public const int MonsterDetected = 100031;
        public const int RetreatOrder = 100032;
    }

    private class NpcText
    {
        public const int MuselIntro = 20402;
        public const int MuselIdle = 20403;
        public const int IdeIntro = 20404;
        public const int IdeIdle = 20405;
        public const int IdeReturn = 20406;
        public const int IdeReturnIdle = 20407;
        public const int MuselReturn = 20408;

        public const int GlenisShout0 = 20409;
        public const int GlenisShout1 = 20410;
        public const int NoraShout0 = 20411;

        public const int GlenisIdle = 21119;
        public const int NoraIdle = 20120;
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 15350);
        AddWalletReward(WalletType.Gold, 4618);
        AddWalletReward(WalletType.RiftPoints, 600);

        AddFixedItemReward(ItemId.PremiumFragrantWood, 10);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.InitialGroup, Stage.ElanWaterGrove, 14, 0, QuestEnemyPlacementType.Manual, 
            Enumerable.Range(0, 5)
            .Select(i => LibDdon.Enemy.CreateAuto(EnemyId.Stymphalides, RecommendedLevel, (byte)i))
            .ToList()
        );

        AddEnemies(EnemyGroupId.InitialGroup+1, Stage.ElanWaterGrove, 50, 0, QuestEnemyPlacementType.Manual, new());
        AddEnemies(EnemyGroupId.InitialGroup+2, Stage.ElanWaterGrove, 30, 0, QuestEnemyPlacementType.Manual, new());


        AddEnemies(EnemyGroupId.AmbushGroup, Stage.ElanWaterGrove, 50, 0, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Stymphalides, RecommendedLevel, 0),
            LibDdon.Enemy.CreateAuto(EnemyId.Stymphalides, RecommendedLevel, 1),
            LibDdon.Enemy.CreateAuto(EnemyId.Stymphalides, RecommendedLevel, 2),
            LibDdon.Enemy.CreateAuto(EnemyId.Stymphalides, RecommendedLevel, 3),
            LibDdon.Enemy.CreateAuto(EnemyId.Stymphalides, RecommendedLevel, 4),
            LibDdon.Enemy.CreateAuto(EnemyId.WhiteGriffin0, 75, 5, isBoss:true)
                .SetNamedEnemyParams(NamedParamId.GriffinTitle)
                .SetStartThinkTblNo(2), // Spawns in flying
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdCheckAreaRank(QuestAreaId.ElanWaterGrove, 5);

        // "Speak to the Area Master"
        process0.AddNpcTalkAndOrderBlock(Stage.ProtectorsRetreat, NpcId.Musel0, NpcText.MuselIntro)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, MyQstFlag.Ide);

        // "Go to the rescue site and ask for information"
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, Stage.ElanWaterGrove, 0, 0, NpcId.Ide, NpcText.IdeIntro)
            .AddResultCmdQstTalkChg(NpcId.Musel0, NpcText.MuselIdle);

        // "Investigate the location of the attack"
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.InitialGroup, true)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, MyQstFlag.Glenis)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, MyQstFlag.Nora)
            .AddResultCmdQstTalkChg(NpcId.Ide, NpcText.IdeIdle)
            .AddEnemyGroupId(EnemyGroupId.InitialGroup + 1) // Ensure the ambush is clear
            .AddEnemyGroupId(EnemyGroupId.InitialGroup + 2) // Ensure the ambush is clear
        ;

        // "Sweep away monsters in the middle of a melee"
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.InitialGroup, false)
            .AddResultCmdPlayMessage(NpcText.GlenisShout0, 0);

        // "Defeat the attacking unknown demons"
        // TODO: This doesn't quite trigger correctly compared to video evidence, but it's close enough.
        process0.AddSpawnGroupBlock(QuestAnnounceType.Update, EnemyGroupId.AmbushGroup, true)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.MonsterDetected)
            .AddResultCmdSetAnnounce(QuestAnnounceType.Caution) // Controlling announce order
            .AddResultCmdPlayMessage(NpcText.GlenisShout1, 0)
            .AddCheckCmdEmHpLess(Stage.ElanWaterGrove, 50, 5, 99) // This is probably incorrect.
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.GriffinSpawned)
        ;

        // "Tell Ide about the rebuilding of the investigation team"
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Update, Stage.ElanWaterGrove, 0, 0,NpcId.Ide, NpcText.IdeReturn)
            .AddResultCmdPlayMessage(NpcText.NoraShout0, 0)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.RetreatOrder)
        ;

        // "Report to the Area Master"
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.ProtectorsRetreat, NpcId.Musel0, NpcText.MuselReturn)
            .AddResultCmdQstTalkChg(NpcId.Ide, NpcText.IdeReturnIdle);

        process0.AddProcessEndBlock(true);

        // Extra quest path if you kill the griffin
        var process1 = AddNewProcess(1);
        process1.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdMyQstFlagOn(MyQstFlag.GriffinSpawned)
        ;
            
        process1.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdDieEnemy(Stage.ElanWaterGrove, 50, -1)
        ;  

        process1.AddRawBlock(QuestAnnounceType.None)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.FsmFlag)
            .AddResultCmdQstTalkChg(NpcId.Glenis, NpcText.GlenisIdle)
            .AddResultCmdQstTalkChg(NpcId.Nora, NpcText.NoraIdle)
        ;

    }
}

return new ScriptedQuest();
