using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class AreaGetAreaQuestHintListHandler : GameRequestPacketHandler<C2SAreaGetAreaQuestHintListReq, S2CAreaGetAreaQuestHintListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AreaGetAreaQuestHintListHandler));

        public AreaGetAreaQuestHintListHandler(DdonGameServer server) : base(server)
        {
        }

        // The list in the response is cross-referenced by the client against some list of discovered quests that doesn't seem to populate properly.
        // If you log in with the quest, it'll show up here, but not if you found it this session???
        public override S2CAreaGetAreaQuestHintListRes Handle(GameClient client, C2SAreaGetAreaQuestHintListReq request)
        {
            return new();
        }
    }
}
