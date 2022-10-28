using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetAdventureGuideQuestNoticeHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetAdventureGuideQuestNoticeHandler));


        public QuestGetAdventureGuideQuestNoticeHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_QUEST_GET_ADVENTURE_GUIDE_QUEST_NOTICE_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            //client.Send(GameFull.Dump_162);
            
            IBuffer buffer = new StreamBuffer();
            buffer.WriteInt32(0); // error
            buffer.WriteInt32(0); // result
            buffer.WriteBool(false); // notice
            client.Send(new Packet(PacketId.S2C_QUEST_GET_ADVENTURE_GUIDE_QUEST_NOTICE_RES, buffer.GetAllBytes()));
        }
    }
}
