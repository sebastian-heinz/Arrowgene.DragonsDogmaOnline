using Arrowgene.Ddon.Shared.Entity.Structure;

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
        public uint Id { get; set; } = 0;
        public uint RequesterCharacterId { get; set; }
        public uint RequestedCharacterId { get; set; }
        public ContactListStatus Status { get; set; }
        public ContactListType Type { get; set; }

        public uint GetOtherCharacterId(uint characterId)
        {
            if (this.RequesterCharacterId == characterId) return this.RequestedCharacterId;
            return this.RequesterCharacterId;
        }
        
        public static CDataCommunityCharacterBaseInfo CharacterToCommunityInfo(Character c)
        {
            return new CDataCommunityCharacterBaseInfo()
            {
                CharacterId = c.CharacterId,
                CharacterName = new CDataCharacterName()
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName
                },
                ClanName = "", // TODO get clan

            };

        }
        
        public static CDataFriendInfo CharacterToFriend(Character c, uint unFriendNo)
        {
            return new CDataFriendInfo()
            {
                IsFavorite = false,
                PendingStatus = 0x00, // TODO
                UnFriendNo = unFriendNo, // TODO
                CharacterListElement = new CDataCharacterListElement()
                {
                    OnlineStatus = c.OnlineStatus,
                    MatchingProfile = c.MatchingProfile.Comment,
                    ServerId = c.Server.Id,
                    CommunityCharacterBaseInfo = CharacterToCommunityInfo(c),
                    CurrentJobBaseInfo = new CDataJobBaseInfo()
                    {
                        Job = c.Job,
                        Level = 10
                    },
                    EntryJobBaseInfo = new CDataJobBaseInfo()
                    { // TODO
                        Job = JobId.Hunter,
                        Level = 20
                    }
                }

            };
            
        }
    }
}
