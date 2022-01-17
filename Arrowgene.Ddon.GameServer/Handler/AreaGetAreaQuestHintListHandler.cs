using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class AreaGetAreaQuestHintListHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(AreaGetAreaQuestHintListHandler));


        public AreaGetAreaQuestHintListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_AREA_GET_AREA_QUEST_HINT_LIST_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            client.Send(GameFull.Dump_148);
        }
    }
}
