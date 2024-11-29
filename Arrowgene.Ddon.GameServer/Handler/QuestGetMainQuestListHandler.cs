using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetMainQuestListHandler : GameRequestPacketHandler<C2SQuestGetMainQuestListReq, S2CQuestGetMainQuestListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetMainQuestListHandler));

        public QuestGetMainQuestListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetMainQuestListRes Handle(GameClient client, C2SQuestGetMainQuestListReq request)
        {
            // client.Send(GameFull.Dump_123);

            S2CQuestGetMainQuestListRes res = new S2CQuestGetMainQuestListRes();
            S2CQuestGetMainQuestNtc ntc = new S2CQuestGetMainQuestNtc();
            foreach (var questId in client.Party.QuestState.GetActiveQuestScheduleIds())
            {
                var quest = client.Party.QuestState.GetQuest(questId);
                if (quest != null && quest.QuestType == QuestType.Main)
                {
                    var questState = client.Party.QuestState.GetQuestState(questId);
                    res.MainQuestList.Add(quest.ToCDataQuestList(questState.Step));
                    ntc.MainQuestList.Add(quest.ToCDataMainQuestList(questState.Step));
                }
            }

            client.Party.SendToAllExcept(ntc, client);

            // res.MainQuestList.Add(Quest25);    // Can't find this quest
            // res.MainQuestList.Add(Quest30260); // Hopes Bitter End (White Dragon)
            // res.MainQuestList.Add(Quest30270); // Those Who Follow the Dragon (White Dragon)
            // res.MainQuestList.Add(Quest30410); // Japanese Name (Joseph Historian)

            return res;
        }
    }
}
