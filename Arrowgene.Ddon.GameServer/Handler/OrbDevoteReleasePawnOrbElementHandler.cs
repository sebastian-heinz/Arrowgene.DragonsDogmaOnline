using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.GameServer.Characters;
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
    public class OrbDevoteReleasePawnOrbElementHandler : StructurePacketHandler<GameClient, C2SOrbDevoteReleasePawnOrbElementReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(OrbDevoteReleasePawnOrbElementHandler));

        private OrbUnlockManager _OrbUnlockManager;

        public OrbDevoteReleasePawnOrbElementHandler(DdonGameServer server) : base(server)
        {
            _OrbUnlockManager = server.OrbUnlockManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SOrbDevoteReleasePawnOrbElementReq> req)
        {
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == req.Structure.PawnId).Single();

            _OrbUnlockManager.UnlockDragonForceAugmentationUpgrade(client, pawn, req.Structure.ElementId);
        }
    }
}
