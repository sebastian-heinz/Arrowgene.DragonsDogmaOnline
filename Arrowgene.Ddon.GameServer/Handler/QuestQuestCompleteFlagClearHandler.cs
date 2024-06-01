using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestQuestCompleteFlagClearHandler : GameStructurePacketHandler<C2SQuestQuestCompleteFlagClearReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestQuestCompleteFlagClearHandler));

        public QuestQuestCompleteFlagClearHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SQuestQuestCompleteFlagClearReq> packet)
        {
            var res = new S2CQuestQuestCompleteFlagClearRes()
            {
                QuestScheduleId = packet.Structure.QuestScheduleId
            };

            client.Send(res);
        }
    }
}
