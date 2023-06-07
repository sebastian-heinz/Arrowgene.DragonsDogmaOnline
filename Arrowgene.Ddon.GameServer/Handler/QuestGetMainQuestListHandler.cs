using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetMainQuestListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetMainQuestListHandler));


        public QuestGetMainQuestListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_QUEST_GET_MAIN_QUEST_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            client.Send(GameFull.Dump_123);

            S2CQuestGetMainQuestListRes res = new S2CQuestGetMainQuestListRes();
            res.KeyId = 1;
            res.QuestScheduleId = 1;
            res.QuestId = 1;
            res.NameMsgId = 1;
            res.DetailMsgId = 2;
            res.OrderNpcId = 1;
            res.BaseLevel = 15;
            //client.Send(res);
        }
    }
}
