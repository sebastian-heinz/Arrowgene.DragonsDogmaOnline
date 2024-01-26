using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestSetPriorityQuestHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestSetPriorityQuestHandler));


        public QuestSetPriorityQuestHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_QUEST_SET_PRIORITY_QUEST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            IBuffer inBuffer = new StreamBuffer(packet.Data);
            inBuffer.SetPositionStart();
            uint data0 = inBuffer.ReadUInt32(Endianness.Big);
           // uint data1 = inBuffer.ReadUInt32(Endianness.Big);
          //  uint data2 = inBuffer.ReadUInt32(Endianness.Big);
            Logger.Debug("data0: "+data0+"\n");

//" data1: "+data1+" data2: "+data2+



                client.Send(GameFull.Dump_653);

        }
    }
}
