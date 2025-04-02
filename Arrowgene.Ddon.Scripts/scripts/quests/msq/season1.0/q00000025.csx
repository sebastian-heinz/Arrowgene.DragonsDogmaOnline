/**
 * @brief The Crimson Crystal
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Main;
    public override QuestId QuestId => QuestId.TheCrimsonCrystal;
    public override ushort RecommendedLevel => 10;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestId NextQuestId => QuestId.TheDullGreyArk;

    private class EnemyGroupId
    {
        public const uint BoBoss = 1;
    }

    private class NamedParamId
    {
        public const uint BoneEating = 456; // Bone-Eating <name>
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.MinimumLevel(10));
        AddQuestOrderCondition(QuestOrderCondition.PersonalQuestCleared(QuestId.CraftedTokenOfTheHeart));
        // AddQuestOrderCondition(QuestOrderCondition.ArisenTactics());
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 2500);
        AddWalletReward(WalletType.Gold, 1800);
        AddWalletReward(WalletType.RiftPoints, 250);

        AddFixedItemReward(ItemId.KnightsMantle0, 1);
        AddSelectItemReward(new()
        {
            (ItemId.BronzeSabatons0, 1),
            (ItemId.SkyBlueWaistguard0, 1),
            (ItemId.EliteLegs0, 1),
            (ItemId.MercenaryLegGuards0, 1)
        });
    }

    protected override void InitializeEnemyGroups()
    {
        AddEnemies(EnemyGroupId.BoBoss + 0, Stage.SmallCaveTombs, 3, QuestEnemyPlacementType.Manual, new()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Cyclops0, 10, 0, isBoss: true)
                // .SetBloodOrbs(20)
                .SetBloodOrbs(300) // Some videos show 20 and others 300
                .SetNamedEnemyParams(NamedParamId.BoneEating),
        });
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Leo0, 10853)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, 273)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.AudienceChamber.Mysial)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.AudienceChamber.Leo)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.AudienceChamber.Iris)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheWhiteDragon7)
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.AudienceChamber.TheWhiteDragon0)
            .AddQuestFlag(QuestFlagAction.Clear, QuestFlags.AudienceChamber.TheCrewEndSeason34);
        process0.AddPartyGatherBlock(QuestAnnounceType.Accept, Stage.GrittenFort0, -3082, 0, -183);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.GrittenFort0, 13, 6);
        process0.AddDiscoverGroupBlock(QuestAnnounceType.CheckpointAndUpdate, EnemyGroupId.BoBoss);
        process0.AddDestroyGroupBlock(QuestAnnounceType.Update, EnemyGroupId.BoBoss, false);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.GrittenFort0, NpcId.Vanessa0, 10848)
            .AddResultCmdReleaseAnnounce(ContentsRelease.OrbEnemy, TutorialId.BloodOrbEnemies);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.Leo0, 10850);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.Mysial0, 10851);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddCheckCmdDogmaOrb()
            .AddResultCmdReleaseAnnounce(ContentsRelease.DragonForceAugmentation, TutorialId.ArisensFurtherGrowthandDragonForceAugmentation, QuestFlags.NpcFunctions.DragonForceAugmentation);
        process0.AddProcessEndBlock(true);

        var process1 = AddNewProcess(1);
        process1.AddIsStageNoBlock(QuestAnnounceType.None, Stage.SmallCaveTombs, false);
        process1.AddProcessEndBlock(false)
            .AddResultCmdTutorialDialog(TutorialId.ShortcutBar);
    }
}

return new ScriptedQuest();
