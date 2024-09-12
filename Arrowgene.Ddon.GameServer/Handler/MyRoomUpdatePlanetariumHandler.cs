using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MyRoomUpdatePlanetariumHandler : GameRequestPacketHandler<C2SMyRoomUpdatePlanetariumReq, S2CMyRoomUpdatePlanetariumRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MyRoomUpdatePlanetariumHandler));

        public MyRoomUpdatePlanetariumHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMyRoomUpdatePlanetariumRes Handle(GameClient client, C2SMyRoomUpdatePlanetariumReq request)
        {
            S2CMyRoomUpdatePlanetariumRes res = new S2CMyRoomUpdatePlanetariumRes();

            // TODO: similar to BGM this might have to be returned as part of furniture list 
            
            return res;
        }
    }
}
