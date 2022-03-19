using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpAreaWarpHandler : StructurePacketHandler<GameClient, C2SWarpAreaWarpReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpAreaWarpHandler));

        public WarpAreaWarpHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SWarpAreaWarpReq> packet)
        {
            // TODO: Get character's RP and substract packet.Structure.Price
            // TODO 2: Don't trust packet.Structure.Price and actually check it in DB

            S2CWarpAreaWarpRes obj = new S2CWarpAreaWarpRes();
            obj.WarpPointId = packet.Structure.WarpPointId;
            obj.Rim = 42069; // TODO: Set obj.Rim as the substraction result
            client.Send(obj);
        }
    }
}
