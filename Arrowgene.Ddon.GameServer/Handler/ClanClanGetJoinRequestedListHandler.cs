using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanGetJoinRequestedListHandler : GameRequestPacketHandler<C2SClanClanGetJoinRequestedListReq, S2CClanClanGetJoinRequestedListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanGetJoinRequestedListHandler));

        public ClanClanGetJoinRequestedListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanGetJoinRequestedListRes Handle(GameClient client, C2SClanClanGetJoinRequestedListReq request)
        {
            // TODO: Implement.
            // client.Send(InGameDump.Dump_69);
            return new();
        }
    }
}
