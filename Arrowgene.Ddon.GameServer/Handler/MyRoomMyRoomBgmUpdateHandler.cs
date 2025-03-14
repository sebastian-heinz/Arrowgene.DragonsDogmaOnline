using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MyRoomMyRoomBgmUpdateHandler : GameRequestPacketHandler<C2SMyRoomMyRoomBgmUpdateReq, S2CMyRoomMyRoomBgmUpdateRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MyRoomMyRoomBgmUpdateHandler));

        public MyRoomMyRoomBgmUpdateHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMyRoomMyRoomBgmUpdateRes Handle(GameClient client, C2SMyRoomMyRoomBgmUpdateReq request)
        {
            S2CMyRoomMyRoomBgmUpdateRes res = new S2CMyRoomMyRoomBgmUpdateRes();

            // TODO: maybe store so it's "remembered" for next time when FurnitureList returns BGM 

            return res;
        }
    }
}
