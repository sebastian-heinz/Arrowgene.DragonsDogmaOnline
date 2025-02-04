using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetAreaBonusListHandler : GameRequestPacketHandler<C2SQuestGetAreaBonusListReq, S2CQuestGetAreaBonusListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetAreaBonusListHandler));

        public QuestGetAreaBonusListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetAreaBonusListRes Handle(GameClient client, C2SQuestGetAreaBonusListReq request)
        {
            // TODO: Implement.
            //var res = EntitySerializer.Get<S2CQuestGetAreaBonusListRes>().Read(GameFull.Dump_284.AsBuffer());

            return new();
        }
    }
}
