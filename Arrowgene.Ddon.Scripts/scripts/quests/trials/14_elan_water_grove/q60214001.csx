/**
 * @brief Elan Water Grove Trial: A Friend's Trail (Elan Water Grove AR7)
 */
#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60214001;
    public override ushort RecommendedLevel => 73;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;
    public override StageInfo StageInfo => Stage.ProtectorsRetreat;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.AreaTrialOrMission;

    private class EnemyGroupId
    {
        public const uint MainGroup = 1;
    }

    private class NamedParamId
    {
        public const uint TargetTitle = 1248; // Stone Golem Ambusher
    }

    private class TargetEnemyId
    {
        public const EnemyId Target = EnemyId.Gargoyle;
    }

    private class NpcText
    {
        public const int MuselIntro = 19275;
        public const int MuselIdle = 19276;
        public const int MuselReturn = 19279;

        public const int GlenisIdle = 19277;
        public const int GlenisShout = 19278;
    }

    private class MyQstFlag
    {
        public const uint FsmFlag = 2062;
        public const uint Glenis = 3481;
    }


    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 21850);
        AddWalletReward(WalletType.Gold, 8816);
        AddWalletReward(WalletType.RiftPoints, 1110);

        AddFixedItemReward(ItemId.PremiumFragrantWood, 10);
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.MainGroup + 0, Stage.ElanWaterGrove, 18, QuestEnemyPlacementType.Manual, 
        Enumerable.Range(0, 5).Select(x => LibDdon.Enemy.CreateAuto(TargetEnemyId.Target, RecommendedLevel, (byte)x)
                .SetNamedEnemyParams(NamedParamId.TargetTitle)).ToList());
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);

        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdCheckAreaRank(QuestAreaId.ElanWaterGrove, 6);

        process0.AddNpcTalkAndOrderBlock(Stage.ProtectorsRetreat, NpcId.Musel0, NpcText.MuselIntro);

        // Go check on Glenis
        process0.AddDiscoverGroupBlock(QuestAnnounceType.Accept, EnemyGroupId.MainGroup, true)
            .AddResultCmdQstTalkChg(NpcId.Musel0, NpcText.MuselIdle)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, MyQstFlag.Glenis)
        ;

        // Defeat the demons
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.MainGroup, false)
            .AddResultCmdSetAnnounce(QuestAnnounceType.Caution);

        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.ElanWaterGrove, false)
            .AddResultCmdPlayMessage(NpcText.GlenisShout, 0); // This gets its own block so that it doesn't play back on login.

        // Report to Musel in Protector's Retreat
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.ProtectorsRetreat, NpcId.Musel0, NpcText.MuselReturn)
            .AddResultCmdQstTalkChg(NpcId.Glenis, NpcText.GlenisIdle)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.FsmFlag)
        ;

        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
