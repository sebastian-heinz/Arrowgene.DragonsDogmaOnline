using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetQuestPartyBonusListHandler : GameRequestPacketHandler<C2SQuestGetQuestPartyBonusListReq, S2CQuestGetQuestPartyBonusListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetQuestPartyBonusListHandler));


        public QuestGetQuestPartyBonusListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetQuestPartyBonusListRes Handle(GameClient client, C2SQuestGetQuestPartyBonusListReq request)
        {
            // TODO: Implement.
            //var foo = EntitySerializer.Get<S2CQuestGetQuestPartyBonusListRes>().Read(GameFull.Dump_164.AsBuffer());

            return new();
        }
    }
}
