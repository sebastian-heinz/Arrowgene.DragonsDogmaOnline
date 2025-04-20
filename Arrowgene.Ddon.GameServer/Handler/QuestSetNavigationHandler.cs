using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestSetNavigationHandler : GameRequestPacketHandler<C2SQuestSetNavigationQuestReq, S2CQuestSetNavigationQuestRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestSetNavigationHandler));

        public QuestSetNavigationHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestSetNavigationQuestRes Handle(GameClient client, C2SQuestSetNavigationQuestReq request)
        {
            var quest = QuestManager.GetQuestByScheduleId(request.QuestScheduleId);

            // The UI prints an error if we allow navigation to be set
            // when the player is not in the "base" aka a safe area
            if (StageManager.IsSafeArea(client.Character.Stage))
            {
                var ntc = new S2CQuestSetNavigationQuestNtc()
                {
                    ErrorCode = 0
                };

                var questState = QuestManager.GetQuestStateManager(client, quest).GetQuestState(quest);
                switch (quest.QuestType)
                {
                    case QuestType.Main:
                        ntc.MainQuestOrderList.Add(quest.ToCDataMainQuestOrderList(0));
                        break;
                    case QuestType.WildHunt:
                        ntc.MobHuntQuestOrderList.Add(quest.ToCDataMobHuntQuestOrderList(0));
                        break;
                    case QuestType.Tutorial:
                        ntc.TutorialQuestOrderList.Add(quest.ToCDataTutorialQuestOrderList(0));
                        break;
                    case QuestType.World:
                        ntc.SetQuestOrderList.Add(quest.ToCDataSetQuestOrderList(0, 0));
                        break;
                        // case QuestType.ExpiredQuestList:
                        //    break;
                }
                client.Party.SendToAll(ntc);
            }

            return new S2CQuestSetNavigationQuestRes()
            {
                QuestScheduleId = request.QuestScheduleId,
                IsBase = StageManager.IsSafeArea(client.Character.Stage)
            };
        }
    }
}
