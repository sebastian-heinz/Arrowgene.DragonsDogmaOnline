using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartnerPawnPawnLikabilityRewardListGetHandler : StructurePacketHandler<GameClient, C2SPartnerPawnPawnLikabilityRewardListGetReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartnerPawnPawnLikabilityRewardListGetHandler));

        public PartnerPawnPawnLikabilityRewardListGetHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPartnerPawnPawnLikabilityRewardListGetReq> req)
        {
            S2CPartnerPawnPawnLikabilityRewardListGetRes res = new S2CPartnerPawnPawnLikabilityRewardListGetRes();
            client.Send(res);
        }
    }
}
