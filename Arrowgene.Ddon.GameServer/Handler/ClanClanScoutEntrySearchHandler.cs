using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanScoutEntrySearchHandler : GameRequestPacketHandler<C2SClanClanScoutEntrySearchReq, S2CClanClanScoutEntrySearchRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanScoutEntrySearchHandler));

        public ClanClanScoutEntrySearchHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanScoutEntrySearchRes Handle(GameClient client, C2SClanClanScoutEntrySearchReq request)
        {
            // TODO: Implement.
            return new S2CClanClanScoutEntrySearchRes();
        }
    }
}
