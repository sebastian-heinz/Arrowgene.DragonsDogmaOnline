using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class FriendRemoveFriendHandler : GameStructurePacketHandler<C2SFriendRemoveFriendReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(FriendRemoveFriendHandler));


        public FriendRemoveFriendHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SFriendRemoveFriendReq> packet)
        {

            ContactListEntity relationship = Database.SelectRelationship(packet.Structure.unFriendNo);
            if (relationship == null)
            {
                Logger.Error(client, $"ContactListEntity not found");
                client.Send(
                    new S2CFriendRemoveFriendRes()
                    {
                        Result = -1,
                        Error = (uint)ErrorCode.ERROR_CODE_FRIEND_INVARID_FRIEND_NO
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
                new S2CFriendRemoveFriendRes()
                {
                    Result = 1
                }    
            );

            uint otherCharId = relationship.GetOtherCharacterId(client.Character.CharacterId);
            
            GameClient otherClient = Server.ClientLookup.GetClientByCharacterId(otherCharId);
            if (otherClient != null)
            {
                otherClient.Send(
                    new S2CFriendRemoveFriendNtc()
                    {
                        CharacterId = client.Character.CharacterId
                    }
                );
            }
        }
    }
}
