using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanConciergeGetListHandler : StructurePacketHandler<GameClient, C2SClanClanConciergeGetListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanConciergeGetListHandler));

        public ClanClanConciergeGetListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SClanClanConciergeGetListReq> req)
        {
            S2CClanClanConciergeGetListRes res = new S2CClanClanConciergeGetListRes();
            client.Send(res);
        }
    }
}
