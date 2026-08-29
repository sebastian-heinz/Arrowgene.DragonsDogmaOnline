/**
 * @brief Epitaph Road: Megadosys Region
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.EpitaphRoadMegadosysRegion;
    public override ushort RecommendedLevel => 90;
    public override byte MinimumItemRank => 93;
    public override bool IsDiscoverable => false;
    public override StageInfo StageInfo => Stage.FortressCityMegadoResidentialLevel1;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.QuestUsefulForAdventure;
    public override bool Enabled => false;

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.RequiredItemRank(102));
        AddQuestOrderCondition(QuestOrderCondition.MainQuestCompleted(QuestId.TheFinalBattleOfTheRoyalCapital));
    }

    protected override void InitializeRewards()
    {
        AddPointReward(PointType.ExperiencePoints, 105000);
        AddWalletReward(WalletType.Gold, 11000);
        AddWalletReward(WalletType.RiftPoints, 2000);

        AddFixedItemReward(ItemId.RoyalCrestMedalMegadosysDistrict, 5);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNpcTalkAndOrderBlock(Stage.FortressCityMegadoResidentialLevel1, NpcId.Toyugaru, 28160);
        process0.AddIsStageNoBlock(QuestAnnounceType.Accept, Stage.OldHeroicSpiritShrine)
            .AddWorldManageUnlock(QuestFlags.FortressCityMegadoResidentialLevel.OldEpitaphRoadShrine)
            .AddResultCmdQstTalkChg(NpcId.Toyugaru, 28161);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddResultCmdQstTalkChg(NpcId.Morgan, 28291)
            .AddCheckCommands([
                QuestManager.CheckCommand.TalkNpcChoice(1149, NpcId.Morgan, 0)
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.TalkNpcChoice(1149, NpcId.Morgan, 1)
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.TalkNpc(1149, NpcId.Morgan),
                QuestManager.CheckCommand.DummyNotProgress()
            ]);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddResultCmdQstTalkChg(NpcId.Morgan, 28291) // Don't lose context on checkpoint
            .AddResultCmdCallGreatPurpose(6, 10)
            .AddCheckCommands([
                QuestManager.CheckCommand.StageNo(1150)
            ])
            .AddCheckCommands([
                QuestManager.CheckCommand.TalkNpc(1149, NpcId.Morgan),
                QuestManager.CheckCommand.DummyNotProgress()
            ]);
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddResultCmdTutorialDialog(TutorialId.TheEpitaphRoadOrdeals)
            .AddCheckCommands([
                QuestManager.CheckCommand.IsEnemyFound(1150, 54, 16, 1)
            ]);
        process0.AddRawBlock(QuestAnnounceType.Update)
            .AddCheckCommands([
                QuestManager.CheckCommand.DieEnemy(1150, 54, 16, 1) // Find a way to mark a non-quest enemy later
            ]);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddCheckCmdOmReleaseTouch(Stage.MemoryofMegadosys, 100, 0);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.FortressCityMegadoResidentialLevel1, NpcId.Toyugaru, 28294)
            .AddResultCmdQstTalkDel(NpcId.Morgan);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.TheWhiteDragon, 28296)
            .AddResultCmdQstTalkChg(NpcId.Toyugaru, 28295);
        process0.AddProcessEndBlock(true)
            .AddResultCmdTutorialDialog(TutorialId.TheEpitaphRoadDivineProtectionofHeroicSpirits)
            .AddResultCmdReleaseAnnounce(ContentsRelease.SpecialSkillAugmentation, TutorialId.SkillInheritanceOfUltimateSkills, QuestFlags.NpcFunctions.SpecialSkillAugmentation);
    }
}

return new ScriptedQuest();
