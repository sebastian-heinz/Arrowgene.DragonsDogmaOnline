using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class OrbDevoteGetPawnReleaseOrbElementListHandler : GameRequestPacketHandler<C2SOrbDevoteGetPawnReleaseOrbElementListReq, S2COrbDevoteGetPawnReleaseOrbElementListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(OrbDevoteGetReleaseOrbElementListHandler));

        public OrbDevoteGetPawnReleaseOrbElementListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2COrbDevoteGetPawnReleaseOrbElementListRes Handle(GameClient client, C2SOrbDevoteGetPawnReleaseOrbElementListReq request)
        {
            Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);

            S2COrbDevoteGetPawnReleaseOrbElementListRes response = new S2COrbDevoteGetPawnReleaseOrbElementListRes()
            {
                PawnId = request.PawnId,
                OrbElementList = Database.SelectOrbReleaseElementFromDragonForceAugmentation(pawn.CommonId)
            };
            return response;
        }
    }
}
