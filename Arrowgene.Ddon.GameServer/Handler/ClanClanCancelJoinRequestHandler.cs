using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanCancelJoinRequestHandler : GameRequestPacketHandler<C2SClanClanCancelJoinReq, S2CClanClanCancelJoinRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanCancelJoinRequestHandler));

        public ClanClanCancelJoinRequestHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanCancelJoinRes Handle(GameClient client, C2SClanClanCancelJoinReq request)
        {
            Server.Database.ExecuteInTransaction(conn =>
            {
                Server.Database.DeleteClanRequestByCharacter(client.Character.CharacterId, conn);
            });
            return new S2CClanClanCancelJoinRes();
        }
    }
}
