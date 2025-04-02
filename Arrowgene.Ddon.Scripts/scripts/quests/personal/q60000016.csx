/**
 * @brief Crafted Token of the Heart
 * @cheats
 * - Get items to start craft
 *     /giveitem 7858 1
 *     /giveitem 7974 1
 * - Get the craft result
 *     /giveitem 8963 1
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.Tutorial;
    public override QuestId QuestId => QuestId.CraftedTokenOfTheHeart;
    public override ushort RecommendedLevel => 8;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => true;
    public override StageInfo StageInfo => Stage.AudienceChamber;
    public override QuestAdventureGuideCategory? AdventureGuideCategory => QuestAdventureGuideCategory.QuestUsefulForAdventure;

    public class MyQstFlag
    {
        public const uint MarkNpcVendors = 1;
        public const uint ItemsAcquired = 2;
        public const uint TriggerEvent = 3;
    }

    protected override void InitializeState()
    {
        AddQuestOrderCondition(QuestOrderCondition.SoloWithPawns());
        AddQuestOrderCondition(QuestOrderCondition.PersonalQuestCleared(QuestId.ReliableSourceOfInformation));
    }

    protected override void InitializeRewards()
    {
        AddFixedItemReward(ItemId.RiftstoneShard, 2); // Old videos don't have this reward
        AddFixedItemReward(ItemId.TravelersTights0, 1);
        AddPointReward(PointType.ExperiencePoints, 3000); // Old videos have 1800, new have 3000
        AddWalletReward(WalletType.Gold, 1200);
        AddWalletReward(WalletType.RiftPoints, 180);
    }

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdIsMainQuestClear(QuestId.AServantsPledge);
        process0.AddNpcTalkAndOrderBlock(Stage.AudienceChamber, NpcId.Iris, 10988)
            // Personal quests were added after main quests, so some players might
            // be past the point where Iris is no longer in the audience chamber
            // so temporarily bring her back until this quest is compelted
            .AddQuestFlag(QuestFlagAction.Set, QuestFlags.AudienceChamber.Iris);
        process0.AddIsStageNoBlock(QuestAnnounceType.Accept, Stage.CraftRoom)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.TriggerEvent);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.CraftRoom, NpcId.Sonia, 10990)
            .AddResultCmdTutorialDialog(TutorialId.CraftRoom);
        process0.AddRawBlock(QuestAnnounceType.CheckpointAndUpdate)
            .AddResultCmdReleaseAnnounce(ContentsRelease.Craft, TutorialId.CraftingBasicItems, QuestFlags.NpcFunctions.CraftOfficer)
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.MarkNpcVendors)
            .AddCheckCmdHaveItemAllBag(ItemId.BronzeOre, 1)
            .AddCheckCmdHaveItemAllBag(ItemId.BabyHerb, 1);
        process0.AddCheckBagEventBlock(QuestAnnounceType.CheckpointAndUpdate, ItemId.ZephyrRing, 1)
            .AddCheckCmdCraft()
            .AddQuestFlag(QuestFlagType.MyQst, QuestFlagAction.Set, MyQstFlag.ItemsAcquired);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.CraftRoom, NpcId.Sonia, 10992)
            .AddResultCmdTutorialDialog(TutorialId.CustomizingEquipment);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.TheWhiteDragonTemple0, NpcId.Mayleaf0, 10996);
        process0.AddTalkToNpcBlock(QuestAnnounceType.CheckpointAndUpdate, Stage.AudienceChamber, NpcId.Iris, 14825);
        process0.AddIsStageNoBlock(QuestAnnounceType.None, Stage.AudienceChamber)
            .AddResultCmdReleaseAnnounce(ContentsRelease.MyrmidonsPledge, TutorialId.MyrmidonsPledge, QuestFlags.NpcFunctions.MyrmidonsPledge);
        process0.AddProcessEndBlock(true);

        // Add optional interactions with the vendor NPCs in the crafting hall
        var process1 = AddNewProcess(1);
        process1.AddMyQstFlagsBlock(QuestAnnounceType.None)
            .AddMyQstCheckFlag(MyQstFlag.MarkNpcVendors);
        process1.AddTalkToNpcBlock(QuestAnnounceType.None, Stage.CraftRoom, NpcId.McCormick, 11229)
            .AddCheckCmdMyQstFlagOn(MyQstFlag.ItemsAcquired, commandListIndex: 1);
        process1.AddProcessEndBlock(false);

        var process2 = AddNewProcess(2);
        process2.AddMyQstFlagsBlock(QuestAnnounceType.None)
            .AddMyQstCheckFlag(MyQstFlag.MarkNpcVendors);
        process2.AddTalkToNpcBlock(QuestAnnounceType.None, Stage.CraftRoom, NpcId.Gilliam, 11227)
            .AddCheckCmdMyQstFlagOn(MyQstFlag.ItemsAcquired, commandListIndex: 1);
        process2.AddProcessEndBlock(false);

        // Plays fancy camera event
        var process3 = AddNewProcess(3);
        process3.AddMyQstFlagsBlock(QuestAnnounceType.None)
            .AddMyQstCheckFlag(MyQstFlag.TriggerEvent);
        process3.AddRawBlock(QuestAnnounceType.None)
            .AddCheckCmdSceHitIn(Stage.TheWhiteDragonTemple0, 3, false);
        process3.AddRawBlock(QuestAnnounceType.None)
            .AddResultCmdPlayCameraEvent(Stage.TheWhiteDragonTemple0, 90);
        process3.AddProcessEndBlock(false);
    }
}

return new ScriptedQuest();
