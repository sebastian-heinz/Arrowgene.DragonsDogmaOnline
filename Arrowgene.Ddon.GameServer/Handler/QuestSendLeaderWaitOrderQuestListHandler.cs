using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestSendLeaderWaitOrderQuestListHandler : GameStructurePacketHandler<C2SQuestSendLeaderWaitOrderQuestListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestSendLeaderWaitOrderQuestListHandler));

        public QuestSendLeaderWaitOrderQuestListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SQuestSendLeaderWaitOrderQuestListReq> packet)
        {
            if(packet.Structure.QuestScheduleIdList.Count > 0)
            {
                S2CQuestSendLeaderWaitOrderQuestListNtc ntc = new S2CQuestSendLeaderWaitOrderQuestListNtc() {
                    QuestScheduleIdList = packet.Structure.QuestScheduleIdList
                };
                foreach(GameClient member in client.Party.Members)
                {
                    if(member.Character.Id != member.Party.Leader.Character.Id)
                    {
                        member.Send(ntc);
                    }
                }
            }
            
            client.Send(new S2CQuestSendLeaderWaitOrderQuestListRes());
        }
    }
}
