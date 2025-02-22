namespace Arrowgene.Ddon.Shared.Model.Scheduler
{
    public enum TaskType : uint
    {
        // Types derived from http://ddon.wikidot.com/gameplay:home
        // Possible to add new types other than from this list
        // Try not to change values as this will confuse the scheduler
        // in an existing database (unless they are not being used yet)
        Unknown = 0,
        RevivalGreenGemstones = 1,
        LoginStamps = 2,
        WeakenedStatusRecovery = 3,
        AreaPointReset = 4,
        AreaMasterSupportItems = 5,
        BoardQuestRotation = 6,
        SpecialBoardQuestRotation = 7,
        WorldQuestRotation = 8,
        ClanQuestRotation = 9,
        SubstoryQuestRotation = 10,
        ExtremeMissionRewardUpdate = 11,
        PawnExpedition = 12,
        PawnAffectionIncreaseInteraction = 13,
        PawnTrainingExperiencePoints = 14,
        ClanDungeonReset = 15,
        MandragoraGrowth = 16,
        GoldenTreasureChestReset = 17,
        VioletTreasureChestReset = 18,
        EpitaphRoadRewardsReset = 19,
        AwardBitterblackMazeResetTickets = 20,
        TimeLockedDungeons = 21,
        // Others not from above webpage
        SeasonalEventSchedule = 22,
        RankingBoardReset = 23
    }
}
