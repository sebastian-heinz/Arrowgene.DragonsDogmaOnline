/**
 * @brief Road to Mastery
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.RoadToMastery;
    public override ushort RecommendedLevel => 70;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.TheWhiteDragonTemple0;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.QuestUsefulForAdventure;

    public override bool AcceptRequirementsMet(GameClient client)
    {
        return client.Character.CharacterJobDataList.Any(x => x.Lv >= 70);
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 3337);
        AddWalletReward(WalletType.Gold, 1154);
        AddWalletReward(WalletType.RiftPoints, 150);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.TheWhiteDragonTemple0, NpcId.Archibald0, 20221);
        process0.AddRawBlock(QuestAnnounceType.Accept)
            .AddCheckCmdOpenPpMode()
            .AddResultCmdReleaseAnnounce(ContentsRelease.PlayPoints, TutorialId.PlayPoints);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Archibald0, 20223);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AncientBurialMound);
        process0.AddIsStageNoBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AncientBurialMound, false)
            .AddCheckCmdPpNotLess(1);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Archibald0, 20225);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Renton0, 20227);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddWorldManageUnlock(QuestFlags.NpcFunctions.PlayPointShop)
            .AddResultCmdTutorialDialog(TutorialId.PlayPointShop)
            .AddResultCmdGeneralAnnounce(QuestGeneralAnnounceType.CommonMsg, 100033)
            .AddCheckCmdOpenPpShop();
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Renton0, 20229);
        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
