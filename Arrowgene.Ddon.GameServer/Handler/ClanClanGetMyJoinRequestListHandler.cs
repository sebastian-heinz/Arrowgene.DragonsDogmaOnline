using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanGetMyJoinRequestListHandler : GameRequestPacketHandler<C2SClanClanGetMyJoinRequestListReq, S2CClanClanGetMyJoinRequestListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanGetMyJoinRequestListHandler));

        public ClanClanGetMyJoinRequestListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanGetMyJoinRequestListRes Handle(GameClient client, C2SClanClanGetMyJoinRequestListReq request)
        {
            S2CClanClanGetMyJoinRequestListRes res = new S2CClanClanGetMyJoinRequestListRes();
            Server.Database.ExecuteInTransaction(conn =>
            {
                res.JoinInfo = Server.Database.GetClanRequestsByCharacter(client.Character.CharacterId, conn);
            });
            return res;
        }
    }
}
