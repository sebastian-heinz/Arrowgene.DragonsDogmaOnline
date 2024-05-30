using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BazaarGetCharacterListHandler : GameRequestPacketHandler<C2SBazaarGetCharacterListReq, S2CBazaarGetCharacterListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BazaarGetCharacterListHandler));
        
        public BazaarGetCharacterListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBazaarGetCharacterListRes Handle(GameClient client, C2SBazaarGetCharacterListReq request)
        {
            S2CBazaarGetCharacterListRes response = new S2CBazaarGetCharacterListRes()
            {
                BazaarList = Server.BazaarManager.GetExhibitionsByCharacter(client.Character)
                    .Select(exhibition => exhibition.Info)
                    .ToList()
            };
            return response;
        }
    }
}