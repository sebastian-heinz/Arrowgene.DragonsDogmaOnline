namespace Arrowgene.Ddon.Shared.Model
{
    public enum ContactListStatus : byte
    {
        PendingApproval = 0,
        Accepted = 1,
        Blacklist = 2
    }
        
    public enum ContactListType : byte
    {
        FriendList = 0,
        BlackList = 1
    }
    
    public class ContactListEntity
    {
        public int Id { get; set; } = -1;
        public int RequesterCharacterId { get; set; }
        public int RequestedCharacterId { get; set; }
        public ContactListStatus Status { get; set; }
        public ContactListType Type { get; set; }

        public int GetOtherCharacterId(int characterId)
        {
            if (this.RequesterCharacterId == characterId) return this.RequestedCharacterId;
            return this.RequesterCharacterId;
        }
    }
}
