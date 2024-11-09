using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class FriendRemoveFriendHandler : GameRequestPacketHandler<C2SFriendRemoveFriendReq, S2CFriendRemoveFriendRes>
	{
		private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(FriendRemoveFriendHandler));


		public FriendRemoveFriendHandler(DdonGameServer server) : base(server)
		{
		}

		public override S2CFriendRemoveFriendRes Handle(GameClient client, C2SFriendRemoveFriendReq request)
		{

			ContactListEntity relationship = Database.SelectContactListById(request.FriendNo)
				?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_FRIEND_INVARID_FRIEND_NO, "ContactListEntity not found");

			if (Database.DeleteContactById(relationship.Id) < 1)
			{
				throw new ResponseErrorException(ErrorCode.ERROR_CODE_FRIEND_INVARID_FRIEND_NO, "Problem deleting contact");
			}

			uint otherCharId = relationship.GetOtherCharacterId(client.Character.CharacterId);

			GameClient otherClient = Server.ClientLookup.GetClientByCharacterId(otherCharId);
			if (otherClient != null)
			{
				otherClient.Send(new S2CFriendRemoveFriendNtc()
					{
						CharacterId = client.Character.CharacterId
					});
			}

			return new S2CFriendRemoveFriendRes()
			{
				Result = 1
			};
		}
	}
}
