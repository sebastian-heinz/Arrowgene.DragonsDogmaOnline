using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MyRoomGetOtherRoomPermissionHandler : GameRequestPacketHandler<C2SMyRoomGetOtherRoomPermissionReq, S2CMyRoomGetOtherRoomPermissionRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MyRoomGetOtherRoomPermissionHandler));

        /// <summary>
        /// Fake LayoutId used to store the set MyRoom permission in the database.
        /// </summary>
        public static readonly byte MYROOM_PERMISSION_LAYOUTNO = 202;

        public MyRoomGetOtherRoomPermissionHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMyRoomGetOtherRoomPermissionRes Handle(GameClient client, C2SMyRoomGetOtherRoomPermissionReq request)
        {
            var targetCharacter = Server.ClientLookup.GetClientByCharacterId(request.CharacterId)?.Character 
                ?? throw new ResponseErrorException(Shared.Model.ErrorCode.ERROR_CODE_CHARACTER_DATA_INVALID_CHARACTER_ID);

            var customizations = Server.Database.SelectMyRoomCustomization(targetCharacter.CharacterId);

            return new()
            {
                PermissionSetting = (uint)customizations.FirstOrDefault(x => x.Value == MYROOM_PERMISSION_LAYOUTNO).Key
            };
        }
    }
}
