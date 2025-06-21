/**
 * @brief To the Heroic Spirit Sleeping Path Feryana District
 */

#load "C:\Users\Paul\Git\Arrowgene.DragonsDogmaOnline\Arrowgene.Ddon.Cli\bin\Debug\net9.0\Files\Assets\scripts\libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.HerosRestFeryanaRegion;
    public override ushort RecommendedLevel => 90;
    public override byte MinimumItemRank => 92;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.LookoutCastle1;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.QuestUsefulForAdventure;

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.RequiredItemRank(92));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheBattleOfLookoutCastle));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 42000);
        AddWalletReward(WalletType.Gold, 11000);
        AddWalletReward(WalletType.RiftPoints, 2000);

        AddFixedItemReward(ItemId.RoyalCrestMedalFeryanaDistrict, 3);
        AddFixedItemReward(ItemId.ScrollOfTheStarrySky, 1);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.LookoutCastle1, NpcId.Mustafa, 26049);        
        process0.AddIsStageNoBlock(QuestAnnounceType.Accept, Stage.HeroicSpiritSleepingPathFeryanaWilderness)
            .AddResultCmdReleaseAnnounce(ContentsRelease.None, flagInfo: QuestFlags.LookoutCastle.EpitaphRoadFeryanaWilderness);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.HeroicSpiritSleepingPathFeryanaWilderness, NpcId.Ira1, 26051);
        process0.AddOmInteractEventBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.HeroicSpiritSleepingPathRuins, 101, 0, OmQuestType.WorldManageQuest, OmInteractType.Release);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.LookoutCastle1, NpcId.Mustafa, 26054);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.TheWhiteDragon, 26213);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.AudienceChamber)
            .AddResultCmdReleaseAnnounce(ContentsRelease.SpecialSkillAugmentation, TutorialId.SkillInheritanceOfUltimateSkills, QuestFlags.NpcFunctions.SpecialSkillAugmentation);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
