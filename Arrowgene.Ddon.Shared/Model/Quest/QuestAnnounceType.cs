namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public enum QuestAnnounceType
    {
        Accept = 0,
        Clear = 1,
        Failed = 2,
        Update = 3,
        Discovered = 4,
        Caution = 5,
        Start = 6,
        ExUpdate = 7,
        End = 8,
        StageStart = 9,
        StageClear = 10,
        UrgentUpdate = 11,
        Unknown0 = 12,
        None = 13,
        // Pseudo Update States
        Checkpoint = 14,
        CheckpointAndUpdate = 15,
    }
}
