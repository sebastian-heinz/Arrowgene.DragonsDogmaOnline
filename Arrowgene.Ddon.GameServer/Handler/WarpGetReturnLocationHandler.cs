using Arrowgene.Buffers;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpGetReturnLocationHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpGetReturnLocationHandler));

        public WarpGetReturnLocationHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_WARP_GET_RETURN_LOCATION_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            // Read request
            C2SWarpGetReturnLocationReq req = EntitySerializer.Get<C2SWarpGetReturnLocationReq>().Read(packet.AsBuffer());

            // Write response
            IBuffer resBuffer = new StreamBuffer();
            resBuffer.WriteInt32(0, Endianness.Big); // error
            resBuffer.WriteInt32(0, Endianness.Big); // result

            S2CWarpGetReturnLocationRes res = new S2CWarpGetReturnLocationRes();
            res.jumpLocation.stageId = 0;
            res.jumpLocation.startPos = 0;

            EntitySerializer.Get<S2CWarpGetReturnLocationRes>().Write(resBuffer, res);
            Packet resPacket = new Packet(PacketId.S2C_WARP_GET_RETURN_LOCATION_RES, resBuffer.GetAllBytes());
            client.Send(resPacket);
        }
    }
}