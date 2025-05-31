using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanJoinRequestHandler : GameRequestPacketHandler<C2SClanClanJoinRequest, S2CClanClanJoinRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanJoinRequestHandler));

        public ClanClanJoinRequestHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanJoinRes Handle(GameClient client, C2SClanClanJoinRequest request)
        {
            Server.Database.ExecuteInTransaction(conn =>
            {
                Server.Database.InsertClanRequest(request.ClanId, client.Character.CharacterId, conn);
            });
            return new S2CClanClanJoinRes();
        }
    }
}
