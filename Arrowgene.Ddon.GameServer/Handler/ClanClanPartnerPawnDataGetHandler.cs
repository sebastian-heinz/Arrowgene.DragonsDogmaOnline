using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanPartnerPawnDataGetHandler : StructurePacketHandler<GameClient, C2SClanClanPartnerPawnDataGetReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanPartnerPawnDataGetHandler));

        public ClanClanPartnerPawnDataGetHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SClanClanPartnerPawnDataGetReq> clanPartnerPawnId)
        {
            S2CClanClanPartnerPawnDataGetRes res = new S2CClanClanPartnerPawnDataGetRes();
            res.ClanPartnerPawnId = clanPartnerPawnId.Structure;
            client.Send(res);
        }
    }
}
