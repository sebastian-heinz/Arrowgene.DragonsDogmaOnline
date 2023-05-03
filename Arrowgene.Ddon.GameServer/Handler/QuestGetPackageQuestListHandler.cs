using System.Reflection.Metadata;
using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetPackageQuestListHandler : GameStructurePacketHandler<C2SQuestGetPackageQuestListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetPackageQuestListHandler));


        public QuestGetPackageQuestListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SQuestGetPackageQuestListReq> packet)
        {
            IBuffer buffer = new StreamBuffer();
            buffer.WriteInt32(0);
            buffer.WriteInt32(0);
            buffer.WriteUInt32(packet.Structure.Unk0);
            buffer.WriteUInt32(0);
            client.Send(new Packet(PacketId.S2C_QUEST_GET_PACKAGE_QUEST_LIST_RES, buffer.GetAllBytes()));
            //client.Send(GameFull.Dump_159);
        }
    }
}
