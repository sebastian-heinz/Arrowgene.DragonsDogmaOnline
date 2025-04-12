using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class OrbDevoteReleasePawnOrbElementHandler : GameRequestPacketQueueHandler<C2SOrbDevoteReleasePawnOrbElementReq, S2COrbDevoteReleasePawnOrbElementRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(OrbDevoteReleasePawnOrbElementHandler));

        public OrbDevoteReleasePawnOrbElementHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SOrbDevoteReleasePawnOrbElementReq request)
        {

            Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);

            return Server.OrbUnlockManager.UnlockDragonForceAugmentationUpgrade(client, pawn, request.ElementId);
        }
    }
}
