namespace Arrowgene.Ddon.Shared.Model
{
    public enum QuestType : uint
    {
        None = 0,
        Main = 1,
        World = 2,
        Board = 3,
        Hidden = 4,
        PersonalQuest = 5,
        ExtremeMissions = 6,
        PawnExpedition = 7,
        PartnerPawnPersonalQuests = 8,

#if false
        Main = 0,
        Set = 1,
        Light = 2,
        Tutorial = 3,
        TimeLimited = 4,
        WorldSetting = 5,
        Cycle01 = 6,
        Cycle04 = 7,
        EndContents = 8,
        CycleSubCategory = 9,
        Pawn = 10,
        DebugTool = 11,
#endif
    }
}
