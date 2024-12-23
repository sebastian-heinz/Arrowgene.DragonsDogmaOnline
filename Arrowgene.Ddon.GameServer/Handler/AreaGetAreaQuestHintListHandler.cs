using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class AreaGetAreaQuestHintListHandler : GameRequestPacketHandler<C2SAreaGetAreaQuestHintListReq, S2CAreaGetAreaQuestHintListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AreaGetAreaQuestHintListHandler));

        public AreaGetAreaQuestHintListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CAreaGetAreaQuestHintListRes Handle(GameClient client, C2SAreaGetAreaQuestHintListReq request)
        {
            var pcap = EntitySerializer.Get<S2CAreaGetAreaQuestHintListRes>().Read(GameFull.Dump_148.AsBuffer());

            var res = new S2CAreaGetAreaQuestHintListRes();
            // This is cross-referenced by the client against some list of discovered quests that doesn't seem to populate properly.
            // If you log in with the quest, it'll show up here, but not if you found it this session???
            foreach (var questScheduleId in client.Party.QuestState.AreaQuests((QuestAreaId)request.AreaId))
            {
                var quest = QuestManager.GetQuestByScheduleId(questScheduleId);
                var questHint = new CDataAreaQuestHint()
                {
                    ScheduleId = questScheduleId,
                    Price = quest.BaseLevel * 50,
                    IsSold = true,
                };
                res.AreaQuestHintList.Add(questHint);
            }

            return new();
        }
    }
}
