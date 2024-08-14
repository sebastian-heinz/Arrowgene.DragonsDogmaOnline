#nullable enable
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GpGetCapHandler : GameRequestPacketHandler<C2SGpGetCapReq, S2CGpGetCapRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GpGetCapHandler));

        public GpGetCapHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CGpGetCapRes Handle(GameClient client, C2SGpGetCapReq request)
        {
            S2CGpGetCapRes res = new S2CGpGetCapRes();

            res.CAP = 123;
            
            return res;
        }
    }
}
