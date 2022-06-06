using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceTreasurePointGetListHandler : StructurePacketHandler<GameClient, C2SInstanceTreasurePointGetListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceTreasurePointGetListHandler));

        public InstanceTreasurePointGetListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceTreasurePointGetListReq> req)
        {
            S2CInstanceTreasurePointGetListRes res = new S2CInstanceTreasurePointGetListRes();
            res.ReqData = req.Structure;
            client.Send(res);
        }
    }
}
