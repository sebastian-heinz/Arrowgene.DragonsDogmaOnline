using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanSearchHandler : GameRequestPacketHandler<C2SClanClanSearchReq, S2CClanClanSearchRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanSearchHandler));

        public ClanClanSearchHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanSearchRes Handle(GameClient client, C2SClanClanSearchReq request)
        {
            // TODO: Implement.
            return new S2CClanClanSearchRes();
        }
    }
}
