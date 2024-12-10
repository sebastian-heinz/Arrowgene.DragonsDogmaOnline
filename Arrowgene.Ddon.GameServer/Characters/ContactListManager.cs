using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Characters;

public class ContactListManager
{
    public static Character getCharWithOnlineStatus(DdonGameServer server, IDatabase database, uint charId)
    {
        var otherClient = server.ClientLookup.GetClientByCharacterId(charId);
        Character otherCharacter = null;
        if (otherClient == null || otherClient.Character == null)
        {
            otherCharacter = database.SelectCharacter(charId);
            otherCharacter.OnlineStatus = OnlineStatus.Offline;
        }
        else
        {
            otherCharacter = otherClient.Character;
        }

        return otherCharacter;
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
            ClanName = c.ClanName.ShortName,
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

    public static CDataFriendInfo CharacterToFriend(Character c, uint friendNo, bool isFavorite)
    {
        return new CDataFriendInfo()
        {
            IsFavorite = isFavorite,
            PendingStatus = 0x00, // TODO
            FriendNo = friendNo,
            CharacterListElement = CharacterToListEml(c)

        };

    }
}
