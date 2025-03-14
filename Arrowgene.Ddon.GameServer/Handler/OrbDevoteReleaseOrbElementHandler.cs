using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class OrbDevoteReleaseOrbElementHandler : GameRequestPacketQueueHandler<C2SOrbDevoteReleaseOrbElementReq, S2COrbDevoteReleaseOrbElementRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(OrbDevoteReleaseOrbElementHandler));

        public OrbDevoteReleaseOrbElementHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SOrbDevoteReleaseOrbElementReq request)
        {
            return Server.OrbUnlockManager.UnlockDragonForceAugmentationUpgrade(client, client.Character, request.ElementId);
        }
    }
}
