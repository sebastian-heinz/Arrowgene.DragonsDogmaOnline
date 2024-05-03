using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class OrbDevoteGetPawnReleaseOrbElementListHandler : StructurePacketHandler<GameClient, C2SOrbDevoteGetPawnReleaseOrbElementListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(OrbDevoteGetReleaseOrbElementListHandler));

        private IDatabase _Database;

        public OrbDevoteGetPawnReleaseOrbElementListHandler(DdonGameServer server) : base(server)
        {
            _Database = server.Database;
        }

        public override void Handle(GameClient client, StructurePacket<C2SOrbDevoteGetPawnReleaseOrbElementListReq> req)
        {
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == req.Structure.PawnId).Single();

            S2COrbDevoteGetPawnReleaseOrbElementListRes Response = new S2COrbDevoteGetPawnReleaseOrbElementListRes()
            {
                PawnId = req.Structure.PawnId,
                OrbElementList = _Database.SelectOrbReleaseElementFromDragonForceAugmentation(pawn.CommonId)
            };
            client.Send(Response);
        }
    }
}
