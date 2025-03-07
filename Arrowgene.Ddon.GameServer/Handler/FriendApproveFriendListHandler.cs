using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class FriendApproveFriendListHandler : GameRequestPacketHandler<C2SFriendApproveFriendReq, S2CFriendApproveFriendRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(FriendApproveFriendListHandler));

        public FriendApproveFriendListHandler(DdonGameServer server) : base(server)
        {
        }
    
        public override S2CFriendApproveFriendRes Handle(GameClient client, C2SFriendApproveFriendReq request)
        {
            ContactListEntity relationship = Database.SelectContactsByCharacterId(request.CharacterId, client.Character.CharacterId);
            if (relationship is not { Status: ContactListStatus.PendingApproval })
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_FRIEND_NOT_IN_APPROVING_LIST, $"ContactListEntity not found");
            }
            
            if (request.IsApproved)
            {
                Database.UpdateContact(request.CharacterId, client.Character.CharacterId,
                    ContactListStatus.Accepted, ContactListType.FriendList, false, false);
            }
            else
            {
                Database.DeleteContact(request.CharacterId, client.Character.CharacterId);
            }
            
            Character requestingChar = ContactListManager.getCharWithOnlineStatus(Server, Database, request.CharacterId);
            var result = new S2CFriendApproveFriendRes()
            {
                FriendInfo = ContactListManager.CharacterToFriend(requestingChar, relationship.Id, relationship.IsFavoriteForCharacter(client.Character.CharacterId)),
            };
            
            // TODO: Notify across servers with an announce.
            GameClient requestingClient = Server.ClientLookup.GetClientByCharacterId(request.CharacterId);
            if (requestingClient != null)
            {
                var ntc = new S2CFriendApproveFriendNtc()
                {
                    FriendInfo = ContactListManager.CharacterToFriend(client.Character, relationship.Id, relationship.IsFavoriteForCharacter(request.CharacterId)),
                    IsApproved = request.IsApproved
                };
                requestingClient.Send(ntc);
            }

            return result;
        }
    }
}
