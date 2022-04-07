using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MandragoraGetMyMandragoraHandler : StructurePacketHandler<GameClient, C2SMandragoraGetMyMandragoraReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MandragoraGetMyMandragoraHandler));

        public MandragoraGetMyMandragoraHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SMandragoraGetMyMandragoraReq> req)
        {
            S2CMandragoraGetMyMandragoraRes res = new S2CMandragoraGetMyMandragoraRes();
            res.MandragoraReq = req.Structure;
            res.MyRoomCsv = Server.AssetRepository.MyRoomAsset;
            client.Send(res);
        }
    }
}
