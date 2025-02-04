using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetLevelBonusListHandler : GameRequestPacketHandler<C2SQuestGetLevelBonusListReq, S2CQuestGetLevelBonusListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetLevelBonusListHandler));


        public QuestGetLevelBonusListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetLevelBonusListRes Handle(GameClient client, C2SQuestGetLevelBonusListReq request)
        {
            // TODO: Implement.
            //var res = EntitySerializer.Get<S2CQuestGetLevelBonusListRes>().Read(GameFull.Dump_286.AsBuffer());

            return new();
        }
    }
}
