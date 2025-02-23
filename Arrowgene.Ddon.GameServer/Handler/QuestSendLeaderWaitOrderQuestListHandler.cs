using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestSendLeaderWaitOrderQuestListHandler : GameRequestPacketHandler<C2SQuestSendLeaderWaitOrderQuestListReq, S2CQuestSendLeaderWaitOrderQuestListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestSendLeaderWaitOrderQuestListHandler));

        public QuestSendLeaderWaitOrderQuestListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestSendLeaderWaitOrderQuestListRes Handle(GameClient client, C2SQuestSendLeaderWaitOrderQuestListReq request)
        {
            if (request.QuestScheduleIdList.Count > 0)
            {
                S2CQuestSendLeaderWaitOrderQuestListNtc ntc = new S2CQuestSendLeaderWaitOrderQuestListNtc() {
                    QuestScheduleIdList = request.QuestScheduleIdList
                };

                foreach(GameClient member in client.Party.Clients)
                {
                    if(member.Character.CharacterId != member.Party.Leader.Client.Character.CharacterId)
                    {
                        member.Send(ntc);
                    }
                }
            }

            return new();
        }
    }
}
