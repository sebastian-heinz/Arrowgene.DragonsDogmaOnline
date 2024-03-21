using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BazaarGetItemHistoryInfoHandler : GameStructurePacketHandler<C2SBazaarGetItemHistoryInfoReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BazaarGetItemHistoryInfoHandler));
        
        public BazaarGetItemHistoryInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SBazaarGetItemHistoryInfoReq> packet)
        {
            client.Send(new S2CBazaarGetItemHistoryInfoRes());
        }
    }
}