using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpWarpHandler : PacketHandler<GameClient> {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpGetWarpPointListHandler));


        public WarpWarpHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_WARP_WARP_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            // Read request
            C2SWarpReq req = EntitySerializer.Get<C2SWarpReq>().Read(packet.AsBuffer());

            // Write response
            IBuffer resBuffer = new StreamBuffer();
            resBuffer.WriteInt32(0, Endianness.Big); // error
            resBuffer.WriteInt32(0, Endianness.Big); // result

            // TODO: Figure out what they do
            S2CWarpRes res = new S2CWarpRes();
            res.warpPointID = 0;
            res.rim = 0;

            EntitySerializer.Get<S2CWarpRes>().Write(resBuffer, res);
            Packet resPacket = new Packet(PacketId.S2C_WARP_WARP_RES, resBuffer.GetAllBytes());
            client.Send(resPacket);
        }
    }
}