using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpReleaseWarpPointHandler : StructurePacketHandler<GameClient, C2SWarpReleaseWarpPointReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpReleaseWarpPointHandler));

        public WarpReleaseWarpPointHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SWarpReleaseWarpPointReq> packet)
        {
            S2CWarpReleaseWarpPointRes res = new S2CWarpReleaseWarpPointRes();
            res.WarpPointID = packet.Structure.WarpPointID;
            client.Send(res);
            
            // TODO: Send S2C_WARP_14_0_16_NTC
        }
    }
}