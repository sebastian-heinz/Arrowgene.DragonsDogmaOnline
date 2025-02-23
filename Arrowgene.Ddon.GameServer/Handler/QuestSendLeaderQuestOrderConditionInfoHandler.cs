using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestSendLeaderQuestOrderConditionInfoHandler : GameRequestPacketHandler<C2SQuestSendLeaderQuestOrderConditionInfoReq, S2CQuestSendLeaderQuestOrderConditionInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestSendLeaderQuestOrderConditionInfoHandler));

        public QuestSendLeaderQuestOrderConditionInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestSendLeaderQuestOrderConditionInfoRes Handle(GameClient client, C2SQuestSendLeaderQuestOrderConditionInfoReq request)
        {
            if (request.OrderConditionInfoList.Count > 0)
            {
                S2CQuestSendLeaderQuestOrderConditionInfoNtc ntc = new S2CQuestSendLeaderQuestOrderConditionInfoNtc() {
                    OrderConditionInfoList = request.OrderConditionInfoList
                };

                client.Party.SendToAllExcept(ntc, client);
            }
            
            return new();
        }
    }
}
