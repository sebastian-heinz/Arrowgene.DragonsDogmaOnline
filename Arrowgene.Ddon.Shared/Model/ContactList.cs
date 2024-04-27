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
        public bool RequesterFavorite { get; set; }
        public bool RequestedFavorite { get; set; }

        public uint GetOtherCharacterId(uint characterId)
        {
            if (this.RequesterCharacterId == characterId) return this.RequestedCharacterId;
            return this.RequesterCharacterId;
        }

        public bool IsFavoriteForCharacter(uint characterId)
        {
            if (characterId == this.RequestedCharacterId) return this.RequestedFavorite;
            if (characterId == this.RequesterCharacterId) return this.RequesterFavorite;
            return false;
        }
        
        public void SetFavoriteForCharacter(uint characterId, bool favorite)
        {
            if (characterId == this.RequestedCharacterId)
            {
                this.RequestedFavorite = favorite;
            }
            else if (characterId == this.RequesterCharacterId)
            {
                this.RequesterFavorite = favorite;
            }
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

        public static CDataCharacterListElement CharacterToListEml(Character c)
        {
            return new CDataCharacterListElement()
            {
                OnlineStatus = c.OnlineStatus,
                MatchingProfile = c.MatchingProfile.Comment,
                ServerId = c.Server.Id,
                CommunityCharacterBaseInfo = CharacterToCommunityInfo(c),
                CurrentJobBaseInfo = new CDataJobBaseInfo()
                {
                    Job = c.Job,
                    Level = (byte)(c.ActiveCharacterJobData?.Lv ?? 0x00)
                },
                EntryJobBaseInfo = new CDataJobBaseInfo()
                {
                    // TODO
                    Job = c.MatchingProfile.EntryJob,
                    Level = (byte)(c.MatchingProfile?.EntryJobLevel ?? 0x00)
                }
            };
        }
        
        public static CDataFriendInfo CharacterToFriend(Character c, uint unFriendNo, bool isFavorite)
        {
            return new CDataFriendInfo()
            {
                IsFavorite = isFavorite,
                PendingStatus = 0x00, // TODO
                UnFriendNo = unFriendNo,
                CharacterListElement = CharacterToListEml(c)

            };
            
        }
    }
}
