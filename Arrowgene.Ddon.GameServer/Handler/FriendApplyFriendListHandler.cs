using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class FriendApplyFriendListHandler :  GameStructurePacketHandler<C2SFriendApplyFriendReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(FriendApplyFriendListHandler));


        public FriendApplyFriendListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SFriendApplyFriendReq> packet)
        {
            
            ContactListEntity existingFriend = Server.Database.SelectContactsByCharacterId(packet.Structure.CharacterId, client.Character.CharacterId);
            if (existingFriend != null)
            {
                uint errorCode = (uint)ErrorCode.ERROR_CODE_FAIL;

                if (existingFriend.Type == ContactListType.BlackList && client.Character.CharacterId == existingFriend.RequesterCharacterId)
                {
                    errorCode = (uint)ErrorCode.ERROR_CODE_FRIEND_TARGET_IN_BLACK_LIST;
                }
                else if (existingFriend.Status == ContactListStatus.Accepted)
                {
                    errorCode = (uint)ErrorCode.ERROR_CODE_FRIEND_TARGET_ALREADY_FRIEND;
                }
                else if (existingFriend.Status == ContactListStatus.PendingApproval && client.Character.CharacterId == existingFriend.RequesterCharacterId)
                {
                    errorCode = (uint)ErrorCode.ERROR_CODE_FRIEND_TARGET_ALREADY_APPLYING;
                }
                else if (existingFriend.Status == ContactListStatus.PendingApproval && client.Character.CharacterId == existingFriend.RequestedCharacterId)
                {
                    errorCode = (uint)ErrorCode.ERROR_CODE_FRIEND_TARGET_ALREADY_APPROVING;
                }
                var res = new S2CFriendApplyFriendRes()
                {
                    FriendInfo = new CDataFriendInfo(),
                    Error = errorCode
                };
                Logger.Error(client, $"already in contact list: {packet.Structure.CharacterId} - friend invitation");
                client.Send(res);
                return;
            }
            
            Character requestedChar = Server.Database.SelectCharacter(packet.Structure.CharacterId);
            if (requestedChar == null)
            {
                var res = new S2CFriendApplyFriendRes();
                Logger.Error(client, $"not found CharacterId:{packet.Structure.CharacterId} for friend invitation");
                res.Error = (uint)ErrorCode.ERROR_CODE_FRIEND_TARGET_PARAM_NOT_FOUND;
                client.Send(res);
                return;
            }

            int id = Database.InsertContact(client.Character.CharacterId, requestedChar.CharacterId,
                ContactListStatus.PendingApproval, ContactListType.FriendList, false, false);
            
            if (id < 1)
            {
                var res = new S2CFriendApplyFriendRes()
                {
                    FriendInfo = new CDataFriendInfo()
                };
                Logger.Error(client, $"Problem saving friend request");
                res.Error = (uint)ErrorCode.ERROR_CODE_FAIL;
                client.Send(res);
                return;
            }
            
            CDataFriendInfo requester = ContactListEntity.CharacterToFriend(client.Character, (uint)id, false);
            CDataFriendInfo requested = ContactListEntity.CharacterToFriend(requestedChar, (uint)id, false);
            requester.UnFriendNo = (uint)id;
            requested.UnFriendNo = (uint)id;

            GameClient requestedClient = Server.ClientLookup.GetClientByCharacterId(packet.Structure.CharacterId);
            if (requestedClient != null)
            {
                var ntc = new S2CFriendApplyFriendNtc()
                {
                    FriendInfo = requester
                };
                requestedClient.Send(ntc);
            }

            
            var Result = new S2CFriendApplyFriendRes()
            {
                FriendInfo = requested,
                Result = 0,
                Error = 0,
                    
            };
            
            client.Send(Result);
        }
    }
}
