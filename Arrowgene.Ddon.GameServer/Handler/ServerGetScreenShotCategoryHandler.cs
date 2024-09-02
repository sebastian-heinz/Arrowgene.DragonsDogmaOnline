using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;


namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ServerGetScreenShotCategoryHandler : GameRequestPacketHandler<C2SServerGetScreenShotCategoryReq, S2CServerGetScreenShotCategoryRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AreaGetAreaBaseInfoListHandler));

        public ServerGetScreenShotCategoryHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CServerGetScreenShotCategoryRes Handle(GameClient client, C2SServerGetScreenShotCategoryReq request)
        {
            //This packet is used to provide a list of categories for the built in photo upload UI.
            //That UI uses C2S_PHOTO_PHOTO_GET_AUTH_ADDRESS_REQ to request a URL and authtoken presumably for the photo upload.
            //Returning an empty response here prevents the UI from ever getting requesting that, since a built-in photo upload seems kind of silly, even for 2016.

            return new S2CServerGetScreenShotCategoryRes();
        }
    }
}
