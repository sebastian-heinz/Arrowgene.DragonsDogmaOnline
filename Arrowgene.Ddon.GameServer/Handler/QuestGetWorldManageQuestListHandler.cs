using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetWorldManageQuestListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetWorldManageQuestListHandler));

        public QuestGetWorldManageQuestListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_QUEST_GET_WORLD_MANAGE_QUEST_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            //client.Send(GameFull.Dump_121);
            client.Send(new S2CQuestGetWorldManageQuestListRes());
        }
    }
}
