using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetPriorityQuestHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(QuestGetPriorityQuestHandler));


        public QuestGetPriorityQuestHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_QUEST_GET_PRIORITY_QUEST_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            client.Send(GameFull.Dump_144);
        }
    }
}
