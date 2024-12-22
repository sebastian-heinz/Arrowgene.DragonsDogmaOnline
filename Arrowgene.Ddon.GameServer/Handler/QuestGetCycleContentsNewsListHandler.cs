using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetCycleContentsNewsListHandler : GameRequestPacketHandler<C2SQuestGetCycleContentsNewsListReq, S2CQuestGetCycleContentsNewsListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetCycleContentsNewsListHandler));


        public QuestGetCycleContentsNewsListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetCycleContentsNewsListRes Handle(GameClient client, C2SQuestGetCycleContentsNewsListReq request)
        {
            var pcap = EntitySerializer.Get<S2CQuestGetCycleContentsNewsListRes>().Read(GameFull.Dump_708.AsBuffer());
            return pcap;
        }
    }
}
