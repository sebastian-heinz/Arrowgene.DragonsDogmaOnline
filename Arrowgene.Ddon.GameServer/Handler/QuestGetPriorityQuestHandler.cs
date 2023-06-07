using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetPriorityQuestHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetPriorityQuestHandler));


        public QuestGetPriorityQuestHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_QUEST_GET_PRIORITY_QUEST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            IBuffer buffer = new StreamBuffer();
            buffer.WriteInt32(0);
            buffer.WriteInt32(0);
            buffer.WriteUInt32(0);
            client.Send(new Packet(PacketId.S2C_QUEST_GET_PRIORITY_QUEST_RES, buffer.GetAllBytes()));
            //client.Send(GameFull.Dump_144);
        }
    }
}
