using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetLightQuestList : GameStructurePacketHandler<C2SQuestGetLightQuestListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetLightQuestList));
        
        public QuestGetLightQuestList(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SQuestGetLightQuestListReq> packet)
        {
            client.Send(new S2CQuestGetLightQuestListRes());
        }
    }
}