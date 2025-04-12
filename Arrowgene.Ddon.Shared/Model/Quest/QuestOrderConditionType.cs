namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public enum QuestOrderConditionType : uint
    {
        None0 = 0,
        MinimumLevel = 1, // Param01 = Level
        MinimumVocationLevel = 2, // Param01 = JobId, Param02 = Level
        Solo = 3,
        None1 = 4,
        None2 = 5,
        MainQuestCompleted = 6, // Param01 = QuestId
        ClearPersonalQuest = 7, // Param01 = QuestId
        ClearExtremeMission = 8, // Param01 = QuestId
        AreaRank = 9, // Param01 = AreaId, Param02 = RequiredRank
        SoloWithPawns = 10,
        ArisenTactics = 11, // Arisen Tactics trial from Renton in the white dragon temple?
        PrepareEquipment = 12, // How to prepare adequate equipment from iris
        None3 = 13,
        PartnerPawnInParty = 14,
        None4 = 15, // None
        ItemRank = 16, // Param01 = AvgItemRank
        PocessesItem = 17, // Param01 = Itemid
        SoloWithPawnCount = 18, // Param01 = # Allowed Pawns
        ClearWorldQuest = 19, // Param01 = QuestId
        ClearSubstory = 20, // Param01 = SubstoryId
        MinimumJobLevel = 21, // Param01 = Level
    }
}
