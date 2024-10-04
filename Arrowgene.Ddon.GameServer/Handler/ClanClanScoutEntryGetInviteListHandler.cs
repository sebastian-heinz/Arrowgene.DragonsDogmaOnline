using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanScoutEntryGetInviteListHandler : GameRequestPacketHandler<C2SClanClanScoutEntryGetInviteListReq, S2CClanClanScoutEntryGetInviteListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanScoutEntryGetInviteListHandler));

        public ClanClanScoutEntryGetInviteListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanScoutEntryGetInviteListRes Handle(GameClient client, C2SClanClanScoutEntryGetInviteListReq request)
        {
            // TODO: Implement.
            return new();
        }
    }
}
