using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MyRoomUpdatePlanetariumHandler : StructurePacketHandler<GameClient, C2SMyRoomUpdatePlanetariumReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MyRoomUpdatePlanetariumHandler));

        public MyRoomUpdatePlanetariumHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SMyRoomUpdatePlanetariumReq> req)
        {
            S2CMyRoomUpdatePlanetariumRes res = new S2CMyRoomUpdatePlanetariumRes();
            res.ItemId = req.Structure;
            client.Send(res);
        }
    }
}
