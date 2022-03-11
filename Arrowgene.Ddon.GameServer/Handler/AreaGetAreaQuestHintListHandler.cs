using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class AreaGetAreaQuestHintListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AreaGetAreaQuestHintListHandler));


        public AreaGetAreaQuestHintListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_AREA_GET_AREA_QUEST_HINT_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            IBuffer buffer = new StreamBuffer();
            buffer.WriteUInt32(0);
            buffer.WriteUInt32(0);
            buffer.WriteUInt32(0);
            Packet p = new Packet(PacketId.S2C_AREA_GET_AREA_QUEST_HINT_LIST_RES, buffer.GetAllBytes());
            client.Send(p);
          //  client.Send(GameFull.Dump_148);
        }
    }
}
