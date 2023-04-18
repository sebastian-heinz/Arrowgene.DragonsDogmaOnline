using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpGetReturnLocationHandler : StructurePacketHandler<GameClient, C2SWarpGetReturnLocationReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpGetReturnLocationHandler));

        public WarpGetReturnLocationHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SWarpGetReturnLocationReq> packet)
        {
            S2CWarpGetReturnLocationRes response = new S2CWarpGetReturnLocationRes();
            response.JumpLocation.stageId = 3; // White Dragon Temple
            response.JumpLocation.startPos = 0;

            client.Send(response);
        }
    }
}