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
            // TODO: Implement.
            return new S2CClanClanGetMyJoinRequestListRes();
        }
    }
}
