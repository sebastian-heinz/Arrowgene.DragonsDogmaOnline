using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class OrbDevoteGetReleaseOrbElementListHandler : GameRequestPacketHandler<C2SOrbDevoteGetReleaseOrbElementListReq, S2COrbDevoteGetReleaseOrbElementListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(OrbDevoteGetReleaseOrbElementListHandler));

        public OrbDevoteGetReleaseOrbElementListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2COrbDevoteGetReleaseOrbElementListRes Handle(GameClient client, C2SOrbDevoteGetReleaseOrbElementListReq request)
        {
            S2COrbDevoteGetReleaseOrbElementListRes response = new S2COrbDevoteGetReleaseOrbElementListRes();
            response.OrbElementList = Server.Database.SelectOrbReleaseElementFromDragonForceAugmentation(client.Character.CommonId);
            return response;
        }
    }
}
