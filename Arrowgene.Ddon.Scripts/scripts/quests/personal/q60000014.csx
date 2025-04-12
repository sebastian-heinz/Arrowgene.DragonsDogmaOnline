/**
 * @brief Reliable Source of Information
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.ReliableSourceOfInformation;
    public override ushort RecommendedLevel => 8;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.QuestUsefulForAdventure;

    protected override void InitializeRewards()
    {
        AddFixedItemReward(ItemId.BlessingVest0, 1);
        AddFixedItemReward(ItemId.FoppishLegs4, 1);
        AddPointReward(PointType.ExperiencePoints, 2000);
        AddWalletReward(WalletType.Gold, 1500);
        AddWalletReward(WalletType.RiftPoints, 200);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsMainQuestClear(QuestId.AServantsPledge)
            .AddCheckCmdIsTutorialQuestClear(QuestId.TheArisensAbilities);
        process0.AddNpcTalkAndOrderBlock(Stage.TheWhiteDragonTemple0, NpcId.Seneka0, 11280);
        process0.AddRawBlock(QuestAnnounceType.Accept)
            .AddResultCmdReleaseAnnounce(ContentsRelease.WorldQuests, TutorialId.WorldQuests)
            .AddResultCmdReleaseAnnounce(ContentsRelease.LestaniaNews, TutorialId.LestaniaNews, QuestFlags.NpcFunctions.LestaniaNews)
            .AddCheckCmdOpenNewspaper();
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Seneka0, 14641);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.BlackGrapeInn, NpcId.Alfred, 13806)
            .AddResultCmdReleaseAnnounce(ContentsRelease.PartyPlayers, TutorialId.PartyCreation)
            .AddResultCmdReleaseAnnounce(ContentsRelease.MatchingProfile)
            .AddResultCmdReleaseAnnounce(ContentsRelease.QuickParty);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddResultCmdReleaseAnnounce(ContentsRelease.AreaMastersWorldQuestInfo, TutorialId.PurchasingWorldQuestInformation)
            .AddCheckCmdOpenAreaMaster(QuestAreaId.HidellPlains);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.BlackGrapeInn, NpcId.Alfred, 14519);
        // Temporary step until WorkQuestClearNum works properly always
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.BlackGrapeInn)
            .AddResultCmdTutorialDialog(TutorialId.QuestRecommendedLevel);
        // process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
        //   .AddResultCmdTutorialDialog(TutorialId.QuestRecommendedLevel)
        //   .AddCheckCmdWorldQuestClearNum(QuestAreaId.HidellPlains, 1);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.BlackGrapeInn, NpcId.Alfred, 13809);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Seneka0, 11295)
            .AddResultCmdTutorialDialog(TutorialId.ReapRewardsfortheWorldQuest);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber);
        process0.AddPlayEventBlock(QuestAnnounceType.None, Stage.AudienceChamber, 11, 3);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.AudienceChamber)
            .AddResultCmdTutorialDialog(TutorialId.RewardMissions)
            .AddResultCmdReleaseAnnounce(ContentsRelease.GrandMissions, flagInfo: QuestFlags.NpcFunctions.GrandMission)
            // TODO: Issac, Dooris, Endale and Nayajiku might be unlocked at different points
            .AddResultCmdReleaseAnnounce(ContentsRelease.ExtremeMissions, flagInfo: QuestFlags.NpcFunctions.SenekaExm)
            .AddResultCmdReleaseAnnounce(ContentsRelease.ExtremeMissions, flagInfo: QuestFlags.NpcFunctions.IsaacExm)
            .AddResultCmdReleaseAnnounce(ContentsRelease.ExtremeMissions, flagInfo: QuestFlags.NpcFunctions.DorisExm)
            .AddResultCmdReleaseAnnounce(ContentsRelease.ExtremeMissions, flagInfo: QuestFlags.NpcFunctions.EndaleExm)
            .AddResultCmdReleaseAnnounce(ContentsRelease.ExtremeMissions, flagInfo: QuestFlags.NpcFunctions.NayajikuExm);

        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
