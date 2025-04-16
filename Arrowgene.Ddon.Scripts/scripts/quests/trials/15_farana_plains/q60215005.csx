/**
 * @brief Farana Plains Trial: The Shadow on the Opposite Shore (Farana Plains AR6)
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60215005;
    public override ushort RecommendedLevel => 70;
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
        // Use a different title here than the Griffin, since 400% HP on an overleveled Tarasque makes shaving off even 1% HP really hard.
        public const uint MysteryTitle = 413; // "???"
    }

    private class TargetEnemyId
    {
        public const EnemyId Target = EnemyId.Tarasque;
    }

    private class NpcText
    {
        public const int AMIntro = 20413;
        public const int AMIdle = 20414;
        public const int AMReturn = 20419;

        public const int LorcanIntro = 20415;
        public const int LorcanIdle0 = 20416;
        public const int LorcanReturn = 20417;
        public const int LorcanIdle1 = 20418;

        public const int GlennerShout0 = 20420;
        public const int GlennerIdle = 20468;
        public const int GlenisShout0 = 20421;
        public const int GlenisIdle = 20467;
    }

    private static class Flags
    {
        // MyQst
        public const int FsmNpcs = 2482; // Triggers the NPCs to retreat

        // Layout
        public const int ProgNpc = 4957; // 進行NPC (0, 0)
        public const int CombatNpc = 4958; // 共闘NPC1 (1, 0) && 共闘NPC2 (2, 0)

    }

    private static class GeneralAnnouncements
    {
        public const int RetreatOrder = 100030; // "The order to retreat has been given! -- Regroup in the designated location!"
    }

    private class QuestNpcs
    {
        public const NpcId AreaMaster = NpcId.Razanailt;
        public const NpcId Requester = NpcId.Lorcan;
    }

    private class QuestStages 
    {
        public static readonly StageInfo AreaMasterStage = Stage.DanaCentrum;
        public static readonly StageInfo TargetStage = Stage.FaranaPlains0;
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 15350);
        AddWalletReward(WalletType.Gold, 4618);
        AddWalletReward(WalletType.RiftPoints, 600);

        AddFixedItemReward(ItemId.SparklingWater, 10);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.MainGroup, QuestStages.TargetStage, 3, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(TargetEnemyId.Target, 75, 11, isBoss:true)
                .SetNamedEnemyParams(NamedParamId.MysteryTitle)
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdCheckAreaRank(QuestAreaId.FaranaPlains, 5);

        // 1	"Get the details of the case from the Area Master and try to solve the problem"
        process0.AddNpcTalkAndOrderBlock(QuestStages.AreaMasterStage, QuestNpcs.AreaMaster, NpcText.AMIntro);

        // 2	Go to the research team and ask for details
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.Accept, QuestStages.TargetStage, 0, 0, QuestNpcs.Requester, NpcText.LorcanIntro)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, Flags.ProgNpc)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle);

        // 3	Head to <SPOT 923> for rescue
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.MainGroup)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, Flags.CombatNpc)
            .AddResultCmdQstTalkChg(QuestNpcs.AreaMaster, NpcText.AMIdle)
            .AddResultCmdQstTalkChg(QuestNpcs.Requester, NpcText.LorcanIdle0);

        // 4	Defeat the unknown demons
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddCheckCmdEmHpLess(QuestStages.TargetStage, 3, 11, 99) // This is actually timer-based or something, its unclear from videos.
            .AddResultCmdSetAnnounce(QuestAnnounceType.Caution)
            .AddResultCmdPlayMessage(NpcText.GlennerShout0, 0);

        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsStageNo(QuestStages.TargetStage)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, GeneralAnnouncements.RetreatOrder)
            .AddResultCmdPlayMessage(NpcText.GlenisShout0, 0)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, Flags.FsmNpcs)
        ;

        // 5	Return to the opposite shore and consult <NPC 917>
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, QuestStages.TargetStage, 0, 0, QuestNpcs.Requester, NpcText.LorcanReturn)
            .AddResultCmdQstTalkChg(NpcId.Glenis, NpcText.GlenisIdle) 
            .AddResultCmdQstTalkChg(NpcId.Glenner, NpcText.GlennerIdle)
        ;

        // 6	Report to <NPC 2501> in <SPOT 877>
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, QuestStages.AreaMasterStage, QuestNpcs.AreaMaster, NpcText.LorcanReturn)
        ;

        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
