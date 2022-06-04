using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceTreasurePointGetCategoryListHandler : StructurePacketHandler<GameClient, C2SInstanceTreasurePointGetCategoryListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceTreasurePointGetCategoryListHandler));

        public InstanceTreasurePointGetCategoryListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceTreasurePointGetCategoryListReq> req)
        {
            S2CInstanceTreasurePointGetCategoryListRes res = new S2CInstanceTreasurePointGetCategoryListRes();
            res.ReqData = req.Structure;
            client.Send(res);
        }
    }
}
