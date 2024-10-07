namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public enum QuestType : uint
    {
        All = 0,
        Light = 1,
        Set = 2,
        Main = 3,
        Tutorial = 4,
        Limited = 5,
        CycleContents = 6,
        CycleContentsQuest = 7,
        WorldManage = 8,
        TimeGain = 9, // Queried when logging in
        Unk0 = 10,
        Unk1 = 11, // Queried when logging in
        Unk3 = 15,

        // Pseudo Categories
        Board = 1,
        World = 1, // World should be Set Quest (2) not light
        ExtremeMission = TimeGain,
        WildHunt = Unk3
#if false
// Seems game has 2 different sets of quest IDs
// which one is the right one to use???
        Main = 0,
        Set = 1,
        Light = 2,
        Tutorial = 3,
        TimeLimited = 4,
        WorldSetting = 5,
        Cycle01 = 6,
        Cycle04 = 7,
        CycleEndContents = 8,
        CycleSubCategory = 9,
        Pawn = 10,
        DebugTool = 11,
        ManagerNum = 12,
#endif
    }
}
