using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MyRoomFurnitureListGetHandler : StructurePacketHandler<GameClient, C2SMyRoomFurnitureListGetReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MyRoomFurnitureListGetHandler));

        public MyRoomFurnitureListGetHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SMyRoomFurnitureListGetReq> req)
        {
            S2CMyRoomFurnitureListGetRes res = new S2CMyRoomFurnitureListGetRes();
            res.MyRoomCsv = Server.AssetRepository.MyRoomAsset;
            client.Send(res);
        }
    }
}
