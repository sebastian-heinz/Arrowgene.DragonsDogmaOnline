using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartnerPawnPawnLikabilityReleasedRewardListGetHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartnerPawnPawnLikabilityReleasedRewardListGetHandler));


        public PartnerPawnPawnLikabilityReleasedRewardListGetHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_PARTNER_PAWN_PAWN_LIKABILITY_RELEASED_REWARD_LIST_GET_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            client.Send(InGameDump.Dump_89);
        }
    }
}
