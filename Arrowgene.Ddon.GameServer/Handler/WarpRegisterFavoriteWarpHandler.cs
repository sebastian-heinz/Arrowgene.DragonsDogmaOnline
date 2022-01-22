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
    public class WarpRegisterFavoriteWarpHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpRegisterFavoriteWarpHandler));

        public WarpRegisterFavoriteWarpHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_WARP_REGISTER_FAVORITE_WARP_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            // Read request
            C2SWarpRegisterFavoriteWarpReq req = EntitySerializer.Get<C2SWarpRegisterFavoriteWarpReq>().Read(packet.AsBuffer());

            // Write response
            IBuffer resBuffer = new StreamBuffer();
            resBuffer.WriteInt32(0, Endianness.Big); // error
            resBuffer.WriteInt32(0, Endianness.Big); // result

            // TODO: Figure out what they do
            S2CWarpRegisterFavoriteWarpRes res = new S2CWarpRegisterFavoriteWarpRes();
            res.slotNo = 0;
            res.warpPointID = 0;

            EntitySerializer.Get<S2CWarpRegisterFavoriteWarpRes>().Write(resBuffer, res);
            Packet resPacket = new Packet(PacketId.S2C_WARP_REGISTER_FAVORITE_WARP_RES, resBuffer.GetAllBytes());
            client.Send(resPacket);
        }
    }
}