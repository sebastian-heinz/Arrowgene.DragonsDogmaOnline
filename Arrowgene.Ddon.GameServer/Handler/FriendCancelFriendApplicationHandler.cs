using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class FriendCancelFriendApplicationHandler : GameRequestPacketHandler<C2SFriendCancelFriendApplicationReq, S2CFriendCancelFriendApplicationRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(FriendCancelFriendApplicationHandler));

        public FriendCancelFriendApplicationHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CFriendCancelFriendApplicationRes Handle(GameClient client, C2SFriendCancelFriendApplicationReq request)
        {
            ContactListEntity relationship = Database.SelectContactsByCharacterId(client.Character.CharacterId, request.CharacterId);
            if (relationship is not { Status: ContactListStatus.PendingApproval })
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_FRIEND_NOT_IN_APPLYING_LIST, $"ContactListEntity not found");
            }

            if (Database.DeleteContactById(relationship.Id) < 1)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_FRIEND_INVARID_FRIEND_NO, $"Problem deleting contact");
            }
            
            GameClient otherClient = Server.ClientLookup.GetClientByCharacterId(request.CharacterId);
            if (otherClient != null)
            {
                otherClient.Send(
                    new S2CFriendCancelFriendApplicationNtc()
                    {
                        CharacterId = client.Character.CharacterId
                    }
                );
            }

            return new S2CFriendCancelFriendApplicationRes()
            {
                Result = 1
            };
        }
    }
}
