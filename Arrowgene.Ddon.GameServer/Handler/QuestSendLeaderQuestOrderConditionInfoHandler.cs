using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestSendLeaderQuestOrderConditionInfoHandler : GameStructurePacketHandler<C2SQuestSendLeaderQuestOrderConditionInfoReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestSendLeaderQuestOrderConditionInfoHandler));

        public QuestSendLeaderQuestOrderConditionInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SQuestSendLeaderQuestOrderConditionInfoReq> packet)
        {
            if(packet.Structure.OrderConditionInfoList.Count > 0)
            {
                S2CQuestSendLeaderQuestOrderConditionInfoNtc ntc = new S2CQuestSendLeaderQuestOrderConditionInfoNtc() {
                    OrderConditionInfoList = packet.Structure.OrderConditionInfoList
                };
                foreach(GameClient member in client.Party.Clients)
                {
                    if(member.Character.CharacterId != member.Party.Leader.Client.Character.CharacterId)
                    {
                        // Disconnects you for some reason
                        //member.Send(ntc);
                    }
                }
            }
            
            client.Send(new S2CQuestSendLeaderQuestOrderConditionInfoRes());
        }
    }
}
