using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetQuestPartyBonusListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetQuestPartyBonusListHandler));


        public QuestGetQuestPartyBonusListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_QUEST_GET_QUEST_PARTY_BONUS_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            IBuffer buffer = new StreamBuffer();
            buffer.WriteUInt32(0);
            buffer.WriteUInt32(0);
            buffer.WriteUInt32(0);
            Packet p = new Packet(PacketId.S2C_QUEST_GET_QUEST_PARTY_BONUS_LIST_RES, buffer.GetAllBytes());
            client.Send(p);
          //  client.Send(GameFull.Dump_164);
        }
    }
}
