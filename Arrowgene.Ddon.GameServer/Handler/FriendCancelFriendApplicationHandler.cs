using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class FriendCancelFriendApplicationHandler : GameStructurePacketHandler<C2SFriendCancelFriendApplicationReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(FriendCancelFriendApplicationHandler));


        public FriendCancelFriendApplicationHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SFriendCancelFriendApplicationReq> packet)
        {
            ContactListEntity relationship = Database.SelectContactsByCharacterId(client.Character.CharacterId, packet.Structure.CharacterId);
            if (relationship is not { Status: ContactListStatus.PendingApproval })
            {
                Logger.Error(client, $"ContactListEntity not found");
                client.Send(
                    new S2CFriendRemoveFriendRes()
                    {
                        Result = -1,
                        Error = (uint)ErrorCode.ERROR_CODE_FRIEND_NOT_IN_APPLYING_LIST
                    }
                );
                return;
            }

            if (Database.DeleteContactById(relationship.Id) < 1)
            {
                Logger.Error(client, $"Problem deleting contact");
                client.Send(
                    new S2CFriendRemoveFriendRes()
                    {
                        Result = -1,
                        Error = (uint)ErrorCode.ERROR_CODE_FRIEND_INVARID_FRIEND_NO
                    }
                );
                return;
            }
            
            client.Send(
                new S2CFriendCancelFriendApplicationRes()
                {
                    Result = 1
                }
            );
            
            GameClient otherClient = Server.ClientLookup.GetClientByCharacterId(packet.Structure.CharacterId);
            if (otherClient != null)
            {
                otherClient.Send(
                    new S2CFriendCancelFriendApplicationNtc()
                    {
                        CharacterId = client.Character.CharacterId
                    }
                );
            }
        }
    }
}
