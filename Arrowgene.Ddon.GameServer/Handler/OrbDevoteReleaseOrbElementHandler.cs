using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class OrbDevoteReleaseOrbElementHandler : GameStructurePacketHandler<C2SOrbDevoteReleaseOrbElementReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(OrbDevoteReleaseOrbElementHandler));

        private OrbUnlockManager _OrbUnlockManager;

        public OrbDevoteReleaseOrbElementHandler(DdonGameServer server) : base(server)
        {
            _OrbUnlockManager = server.OrbUnlockManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SOrbDevoteReleaseOrbElementReq> Request)
        {
            _OrbUnlockManager.UnlockDragonForceAugmentationUpgrade(client, client.Character, Request.Structure.ElementId);            
        }
    }
}
