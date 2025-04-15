/**
 * @brief Farana Plains Trial: The Mystery of the Red Light (Farana Plains AR4)
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => (QuestId)60215006;
    public override ushort RecommendedLevel => 70;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;
    public override StageInfo StageInfo => Stage.DanaCentrum;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.AreaTrialOrMission;

    private class NpcText
    {
        public const int Razanailt0 = 20432;
        public const int Razanailt1 = 20433;
        public const int Oria0 = 20434;
        public const int Oria1 = 20435;
        public const int ACRS0 = 20436;
        public const int ACRS1 = 20437;
        public const int ACRS2 = 20438;
        public const int ACRS3 = 20439;
        public const int Razanailt2 = 20442;
    }
    
    private static class Flags
    {
        // Layout
        public const int ProgNpc = 4969; // 進行NPC１ (0, 0)
        public const int SearchOm = 4970; // 調べるOM (1, 0)
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 15350);
        AddWalletReward(WalletType.Gold, 4618);
        AddWalletReward(WalletType.RiftPoints, 600);

        AddFixedItemReward(ItemId.WhiteBoneRock, 10);
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.SoloWithPawns());
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);

        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdCheckAreaRank(QuestAreaId.FaranaPlains, 1);

        // 1	"Investigate the nature of the red light as per the Area Master's request."
        process0.AddNpcTalkAndOrderBlock(Stage.DanaCentrum, NpcId.Razanailt, NpcText.Razanailt0);

        // 2	Hear about sightings from <NPC 2509>
        process0.AddTalkToNpcBlock(QuestAnnounceType.Accept, Stage.FaranaPlains0, NpcId.Oria, NpcText.Oria0)
            .AddResultCmdQstTalkChg(NpcId.Razanailt, NpcText.Razanailt1)
        ;

        // 3	"Hear about the situation under investigation from <NPC 531>"
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FaranaPlains0, 0, 0, NpcId.ArisenCorpsRegimentalSoldier8, NpcText.ACRS0)
            .AddResultCmdQstTalkChg(NpcId.Razanailt, NpcText.Razanailt1)
            .AddResultCmdQstTalkChg(NpcId.Oria, NpcText.Oria1)
            .AddQuestFlag(QuestFlagType.QstLayout, QuestFlagAction.Set, Flags.ProgNpc)
        ;

        // 4	Investigate the identity of the red spectral light
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddCheckCmdIsOneOffGather()
            .AddResultCmdQstTalkChg(NpcId.ArisenCorpsRegimentalSoldier8, NpcText.ACRS1)
            .AddResultCmdReleaseAnnounce(ContentsRelease.AreaInvestigation, TutorialId.AreaVisualSurvey)
        ;

        // 5	"Tell <NPC 531> what you've discovered"
        process0.AddNewTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FaranaPlains0, 0, 0, NpcId.ArisenCorpsRegimentalSoldier8, NpcText.ACRS2)
        ;

        // 6	"Report to <NPC 2501> the Area Master"
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.DanaCentrum, NpcId.Razanailt, NpcText.Razanailt2)
            .AddResultCmdQstTalkChg(NpcId.ArisenCorpsRegimentalSoldier8, NpcText.ACRS3)
        ;

        process0.AddProcessEndBlock(true);
    }
}

return new ScriptedQuest();
