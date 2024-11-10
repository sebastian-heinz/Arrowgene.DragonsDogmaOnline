using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class FriendGetFriendListHandler : GameRequestPacketHandler<C2SFriendGetFriendListReq, S2CFriendGetFriendListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(FriendGetFriendListHandler));

        public FriendGetFriendListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CFriendGetFriendListRes Handle(GameClient client, C2SFriendGetFriendListReq request)
        {

            S2CFriendGetFriendListRes res = new();

            List<(ContactListEntity, CDataCharacterListElement)> friends = Database.SelectFullContactListByCharacterId(client.Character.CharacterId);

            foreach ((ContactListEntity contact, CDataCharacterListElement character) in friends)
            {
                if (contact.Type != ContactListType.FriendList)
                {
                    continue;
                }

                if (contact.Status == ContactListStatus.Accepted)
                {
                    res.FriendInfoList.Add(new CDataFriendInfo()
                    {
                        CharacterListElement = character,
                        PendingStatus = 0, // TODO
                        IsFavorite = contact.IsFavoriteForCharacter(client.Character.CharacterId),
                        FriendNo = contact.Id,
                    });
                }
                else if (contact.Status == ContactListStatus.PendingApproval)
                {
                    if (contact.RequesterCharacterId == client.Character.CharacterId)
                    {
                        res.ApplyingCharacterList.Add(character.CommunityCharacterBaseInfo);
                    }
                    else if (contact.RequestedCharacterId == client.Character.CharacterId)
                    {
                        res.ApprovingCharacterList.Add(character.CommunityCharacterBaseInfo);
                    }
                }
            }

            return res;
        }
    }
}
