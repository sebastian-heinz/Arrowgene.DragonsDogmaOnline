using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanConciergeUpdateHandler : StructurePacketHandler<GameClient, C2SClanClanConciergeUpdateReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanConciergeUpdateHandler));

        public ClanClanConciergeUpdateHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SClanClanConciergeUpdateReq> req)
        {
            S2CClanClanConciergeUpdateRes res = new S2CClanClanConciergeUpdateRes();
            res.ConciergeUpdate = req.Structure;
            res.CP = 1110;
            client.Send(res);
        }
    }
}
