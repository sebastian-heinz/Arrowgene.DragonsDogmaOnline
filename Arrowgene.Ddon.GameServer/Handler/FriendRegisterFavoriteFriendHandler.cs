using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class FriendRegisterFavoriteFriendHandler : GameRequestPacketHandler<C2SFriendRegisterFavoriteFriendReq, S2CFriendRegisterFavoriteFriendRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(FriendRegisterFavoriteFriendHandler));


        public FriendRegisterFavoriteFriendHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CFriendRegisterFavoriteFriendRes Handle(GameClient client, C2SFriendRegisterFavoriteFriendReq request)
        {
            ContactListEntity r = Database.SelectContactListById(request.FriendNo)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_FRIEND_INVARID_FRIEND_NO, "ContactListEntity not found");

            r.SetFavoriteForCharacter(client.Character.CharacterId, request.IsFavorite);
            Database.UpdateContact(r.RequesterCharacterId, r.RequestedCharacterId, r.Status, r.Type,
                r.RequesterFavorite, r.RequestedFavorite);

            return new S2CFriendRegisterFavoriteFriendRes()
            {
                Result = 1
            };
        }
    }
}
