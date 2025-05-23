using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestLeaderQuestProgressRequestHandler : GameStructurePacketHandler<C2SQuestLeaderQuestProgressRequestReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestLeaderQuestProgressRequestHandler));
        
        public QuestLeaderQuestProgressRequestHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SQuestLeaderQuestProgressRequestReq> packet)
        {
            client.Party.Leader.Client.Send(new S2CQuestLeaderQuestProgressRequestNtc()
            {
                RequestCharacterId = client.Character.CharacterId,
                QuestScheduleId = packet.Structure.QuestScheduleId,
                ProcessNo = packet.Structure.ProcessNo,
                SequenceNo = packet.Structure.SequenceNo,
                BlockNo = packet.Structure.BlockNo
            });

            client.Send(new S2CQuestLeaderQuestProgressRequestRes()
            {
                QuestProgressResult = 0, // TODO: Proper data
                QuestScheduleId = packet.Structure.QuestScheduleId,
                ProcessNo = packet.Structure.ProcessNo
            });
        }
    }
}
