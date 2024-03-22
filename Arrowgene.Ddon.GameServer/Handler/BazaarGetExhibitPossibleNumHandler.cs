using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BazaarGetExhibitPossibleNumHandler : GameStructurePacketHandler<C2SBazaarGetExhibitPossibleNumReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BazaarGetExhibitPossibleNumHandler));
        
        public BazaarGetExhibitPossibleNumHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SBazaarGetExhibitPossibleNumReq> packet)
        {
            client.Send(new S2CBazaarGetExhibitPossibleNumRes());
        }
    }
}