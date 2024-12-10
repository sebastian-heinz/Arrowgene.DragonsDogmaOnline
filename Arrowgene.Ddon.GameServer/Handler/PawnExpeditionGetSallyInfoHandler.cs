using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnExpeditionGetSallyInfoHandler : GameRequestPacketHandler<C2SPawnExpeditionGetSallyInfoReq, S2CPawnExpeditionGetSallyInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetNoraPawnListHandler));


        public PawnExpeditionGetSallyInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnExpeditionGetSallyInfoRes Handle(GameClient client, C2SPawnExpeditionGetSallyInfoReq request)
        {
            // TODO: Implement.
            return new();
        }
    }
}
