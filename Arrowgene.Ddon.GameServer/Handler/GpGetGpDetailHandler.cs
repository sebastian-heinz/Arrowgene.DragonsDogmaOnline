using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GpGetGpDetailHandler : GameRequestPacketHandler<C2SGpGetGpDetailReq, S2CGpGetGpDetailRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GpGetGpDetailHandler));

        public GpGetGpDetailHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CGpGetGpDetailRes Handle(GameClient client, C2SGpGetGpDetailReq packet)
        {
            return new S2CGpGetGpDetailRes();
        }
    }
}
