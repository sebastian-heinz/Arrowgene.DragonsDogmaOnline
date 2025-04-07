using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MyRoomSetOtherRoomPermissionHandler : GameRequestPacketHandler<C2SMyRoomSetOtherRoomPermissionReq, S2CMyRoomSetOtherRoomPermissionRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MyRoomSetOtherRoomPermissionHandler));

        public MyRoomSetOtherRoomPermissionHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMyRoomSetOtherRoomPermissionRes Handle(GameClient client, C2SMyRoomSetOtherRoomPermissionReq request)
        {
            Server.Database.UpsertMyRoomCustomization(client.Character.CharacterId, MyRoomGetOtherRoomPermissionHandler.MYROOM_PERMISSION_LAYOUTNO, request.PermissionSetting);

            return new();
        }
    }
}
