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
        Board = Light,
        World = Set,
        ExtremeMission = TimeGain, // Unsure if this is the proper category
        WildHunt = Unk3
    }
}
