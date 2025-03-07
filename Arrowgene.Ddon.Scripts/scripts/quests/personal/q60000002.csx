/**
 * @brief Gathering in the Clan Tavern
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.GatheringInTheClanTavern;
    public override ushort RecommendedLevel => 8;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.QuestUsefulForAdventure;

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.PersonalQuestCleared(QuestId.ReliableSourceOfInformation));
    }

    protected override void InitializeRewards()
    {
        AddFixedItemReward(ItemId.CombatHands0, 1);
        AddPointReward(PointType.ExperiencePoints, 600);
        AddWalletReward(WalletType.Gold, 400);
        AddWalletReward(WalletType.RiftPoints, 100);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsMainQuestClear(QuestId.AServantsPledge);
        process0.AddNpcTalkAndOrderBlock(Stage.TheWhiteDragonTemple0, NpcId.Freddy, 14818);
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.TheWhiteDragonTemple0, NpcId.Kibiza, 14820);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.TheWhiteDragonTemple0)
            .AddResultCmdReleaseAnnounce(ContentsRelease.CreateandJoinClans, TutorialId.Clans, QuestFlags.NpcFunctions.ClanManagement);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
