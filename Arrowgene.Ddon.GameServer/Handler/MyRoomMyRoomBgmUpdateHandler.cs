using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MyRoomMyRoomBgmUpdateHandler : StructurePacketHandler<GameClient, C2SMyRoomMyRoomBgmUpdateReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MyRoomMyRoomBgmUpdateHandler));

        public MyRoomMyRoomBgmUpdateHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SMyRoomMyRoomBgmUpdateReq> req)
        {
            S2CMyRoomMyRoomBgmUpdateRes res = new S2CMyRoomMyRoomBgmUpdateRes();
            res.ItemId = req.Structure;
            client.Send(res);
        }
    }
}
