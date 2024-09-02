#nullable enable

using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BazaarProceedsHandler : GameRequestPacketHandler<C2SBazaarProceedsReq, S2CBazaarProceedsRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BazaarProceedsHandler));
        
        public BazaarProceedsHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBazaarProceedsRes Handle(GameClient client, C2SBazaarProceedsReq request)
        {
            Server.BazaarManager.Proceeds(client, request.BazaarId, request.ItemStorageIndicateNum);
            return new S2CBazaarProceedsRes
            {
                BazaarId = request.BazaarId
            };
        }
    }
}