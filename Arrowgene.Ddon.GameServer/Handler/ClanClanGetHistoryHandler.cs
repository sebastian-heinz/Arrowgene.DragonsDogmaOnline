using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanGetHistoryHandler : GameRequestPacketHandler<C2SClanClanGetHistoryReq, S2CClanClanGetHistoryRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanGetMyJoinRequestListHandler));

        public ClanClanGetHistoryHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanGetHistoryRes Handle(GameClient client, C2SClanClanGetHistoryReq request)
        {
            // TODO: Implement.
            return new S2CClanClanGetHistoryRes();
        }
    }
}
