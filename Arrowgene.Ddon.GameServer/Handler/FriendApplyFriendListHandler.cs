using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class FriendApplyFriendListHandler : GameRequestPacketHandler<C2SFriendApplyFriendReq, S2CFriendApplyFriendRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(FriendApplyFriendListHandler));


        public FriendApplyFriendListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CFriendApplyFriendRes Handle(GameClient client, C2SFriendApplyFriendReq request)
        {

            ContactListEntity existingFriend = Server.Database.SelectContactsByCharacterId(request.CharacterId, client.Character.CharacterId);
            if (existingFriend != null)
            {
                ErrorCode errorCode = ErrorCode.ERROR_CODE_FAIL;

                if (existingFriend.Type == ContactListType.BlackList && client.Character.CharacterId == existingFriend.RequesterCharacterId)
                {
                    errorCode = ErrorCode.ERROR_CODE_FRIEND_TARGET_IN_BLACK_LIST;
                }
                else if (existingFriend.Status == ContactListStatus.Accepted)
                {
                    errorCode = ErrorCode.ERROR_CODE_FRIEND_TARGET_ALREADY_FRIEND;
                }
                else if (existingFriend.Status == ContactListStatus.PendingApproval && client.Character.CharacterId == existingFriend.RequesterCharacterId)
                {
                    errorCode = ErrorCode.ERROR_CODE_FRIEND_TARGET_ALREADY_APPLYING;
                }
                else if (existingFriend.Status == ContactListStatus.PendingApproval && client.Character.CharacterId == existingFriend.RequestedCharacterId)
                {
                    errorCode = ErrorCode.ERROR_CODE_FRIEND_TARGET_ALREADY_APPROVING;
                }

                throw new ResponseErrorException(errorCode, $"already in contact list: {request.CharacterId} - friend invitation.");
            }

            Character requestedChar = Server.ClientLookup.GetClientByCharacterId(request.CharacterId)?.Character 
                ?? ContactListManager.getCharWithOnlineStatus(Server, Database, request.CharacterId)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_FRIEND_TARGET_PARAM_NOT_FOUND, $"not found CharacterId:{request.CharacterId} for friend invitation.");

            int id = Database.InsertContact(client.Character.CharacterId, requestedChar.CharacterId,
                ContactListStatus.PendingApproval, ContactListType.FriendList, false, false);

            if (id < 1)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_FAIL, $"Problem saving friend request.");
            }

            CDataFriendInfo requester = ContactListManager.CharacterToFriend(client.Character, (uint)id, false);
            CDataFriendInfo requested = ContactListManager.CharacterToFriend(requestedChar, (uint)id, false);
            requester.FriendNo = (uint)id;
            requested.FriendNo = (uint)id;

            GameClient requestedClient = Server.ClientLookup.GetClientByCharacterId(request.CharacterId);
            if (requestedClient != null)
            {
                var ntc = new S2CFriendApplyFriendNtc()
                {
                    FriendInfo = requester
                };
                requestedClient.Send(ntc);
            }

            return new S2CFriendApplyFriendRes()
            {
                FriendInfo = requested
            };
        }
    }
}
