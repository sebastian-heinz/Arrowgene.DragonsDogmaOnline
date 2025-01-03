using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetAdventureGuideQuestListHandler : GameRequestPacketHandler<C2SQuestGetAdventureGuideQuestListReq, S2CQuestGetAdventureGuideQuestListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetQuestPartyBonusListHandler));

        public QuestGetAdventureGuideQuestListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetAdventureGuideQuestListRes Handle(GameClient client, C2SQuestGetAdventureGuideQuestListReq request)
        {
            //var res = EntitySerializer.Get<S2CQuestGetAdventureGuideQuestListRes>().Read(GameFull.Dump_196.AsBuffer());
            return new();
        }
    }
}
