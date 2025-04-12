using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MyRoomUpdatePlanetariumHandler : GameRequestPacketHandler<C2SMyRoomUpdatePlanetariumReq, S2CMyRoomUpdatePlanetariumRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MyRoomUpdatePlanetariumHandler));

        /// <summary>
        /// Fake LayoutId used to store the currently active planetarium in the database.
        /// </summary>
        public static readonly byte MYROOM_PLANETARIUM_LAYOUTNO = 201;

        public MyRoomUpdatePlanetariumHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMyRoomUpdatePlanetariumRes Handle(GameClient client, C2SMyRoomUpdatePlanetariumReq request)
        {
            Server.Database.UpsertMyRoomCustomization(client.Character.CharacterId, MYROOM_PLANETARIUM_LAYOUTNO, request.PlanetariumId); 
            
            return new();
        }
    }
}
