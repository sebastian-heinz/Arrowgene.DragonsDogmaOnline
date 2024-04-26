using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class FriendRegisterFavoriteFriendHandler : GameStructurePacketHandler<C2SFriendRegisterFavoriteFriendReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(FriendRegisterFavoriteFriendHandler));


        public FriendRegisterFavoriteFriendHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SFriendRegisterFavoriteFriendReq> packet)
        {
            ContactListEntity r = Database.SelectRelationship(packet.Structure.unFriendNo);
            if (r == null)
            {
                Logger.Error(client, $"ContactListEntity not found");
                client.Send(
                    new S2CFriendRegisterFavoriteFriendRes()
                    {
                        Result = -1,
                        Error = (uint)ErrorCode.ERROR_CODE_FRIEND_INVARID_FRIEND_NO
                    }
                );
                return;
            }
            
            r.SetFavoriteForCharacter(client.Character.CharacterId, packet.Structure.isFavorite);
            Database.UpsertContact(r.RequesterCharacterId, r.RequestedCharacterId, r.Status, r.Type,
                r.RequesterFavorite, r.RequestedFavorite);
            
            client.Send(
                new S2CFriendRegisterFavoriteFriendRes()
                {
                    Result = 1
                }
            );
        }
    }
}
