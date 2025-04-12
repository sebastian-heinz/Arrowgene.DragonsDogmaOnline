using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MyRoomMyRoomBgmUpdateHandler : GameRequestPacketHandler<C2SMyRoomMyRoomBgmUpdateReq, S2CMyRoomMyRoomBgmUpdateRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MyRoomMyRoomBgmUpdateHandler));

        /// <summary>
        /// Fake LayoutId used to store the currently active BGM in the database.
        /// </summary>
        public static readonly byte MYROOM_BGM_LAYOUTNO = 200;

        public MyRoomMyRoomBgmUpdateHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMyRoomMyRoomBgmUpdateRes Handle(GameClient client, C2SMyRoomMyRoomBgmUpdateReq request)
        {
            Server.Database.UpsertMyRoomCustomization(client.Character.CharacterId, MYROOM_BGM_LAYOUTNO, request.BgmAcquirementNo);

            return new();
        }
    }
}
