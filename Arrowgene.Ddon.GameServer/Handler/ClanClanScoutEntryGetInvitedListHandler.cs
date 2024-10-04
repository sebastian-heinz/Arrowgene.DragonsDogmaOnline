using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanScoutEntryGetInvitedListHandler : GameRequestPacketHandler<C2SClanClanScoutEntryGetInvitedListReq, S2CClanClanScoutEntryGetInvitedListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanScoutEntryGetInvitedListHandler));

        public ClanClanScoutEntryGetInvitedListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanScoutEntryGetInvitedListRes Handle(GameClient client, C2SClanClanScoutEntryGetInvitedListReq request)
        {
            // TODO: Implement.
            return new S2CClanClanScoutEntryGetInvitedListRes();
        }
    }
}
