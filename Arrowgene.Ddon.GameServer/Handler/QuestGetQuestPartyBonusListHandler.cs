using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetQuestPartyBonusListHandler : GameRequestPacketHandler<C2SQuestGetQuestPartyBonusListReq, S2CQuestGetQuestPartyBonusListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetQuestPartyBonusListHandler));


        public QuestGetQuestPartyBonusListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetQuestPartyBonusListRes Handle(GameClient client, C2SQuestGetQuestPartyBonusListReq request)
        {
            // TODO: Implement.
            //  client.Send(GameFull.Dump_164);
            return new();
        }
    }
}
