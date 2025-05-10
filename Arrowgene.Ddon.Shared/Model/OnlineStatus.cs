namespace Arrowgene.Ddon.Shared.Model
{
    public enum OnlineStatus : byte
    {
        None = 0x0,
        Online = 0x1,
        Offline = 0x2,
        Leaving = 0x3,
        Busy = 0x4,
        EntryBoard = 0x5,
        QuickMatch = 0x6,
        Event = 0x7,
        Contents = 0x8,
        PtLeader = 0x9,
        PtMember = 0xA,
        Lost = 0xB,
        ServerMove = 0xC,
        PtPawn = 0xD,
        Num = 0xE
    }
}
