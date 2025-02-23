using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetPawnHistoryListHandler : GameRequestPacketHandler<C2SPawnGetPawnHistoryListReq, S2CPawnGetPawnHistoryListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetPawnHistoryListHandler));

        public PawnGetPawnHistoryListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnGetPawnHistoryListRes Handle(GameClient client, C2SPawnGetPawnHistoryListReq request)
        {
            // TODO: Implement.
            return new();
        }
    }
}
