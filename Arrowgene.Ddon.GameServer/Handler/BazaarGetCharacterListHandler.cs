using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BazaarGetCharacterListHandler : GameStructurePacketHandler<C2SBazaarGetCharacterListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BazaarGetCharacterListHandler));
        
        public BazaarGetCharacterListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SBazaarGetCharacterListReq> packet)
        {
            client.Send(new S2CBazaarGetCharacterListRes());
        }
    }
}