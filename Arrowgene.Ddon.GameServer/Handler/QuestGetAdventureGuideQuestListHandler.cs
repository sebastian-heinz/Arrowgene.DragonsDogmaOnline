using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

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
            // var pcap0 = EntitySerializer.Get<S2CQuestGetAdventureGuideQuestListRes>().Read(GameFull.Dump_196.AsBuffer());
            var result = new S2CQuestGetAdventureGuideQuestListRes();

            // Check All Seasonal Events, Collab Events, Wild Hunts and active quests
            var questScheduleIds = QuestManager.GetQuestsByAdventureGuideCategory(QuestAdventureGuideCategory.CollaborationOrSeasonalQuest);
            questScheduleIds.UnionWith(QuestManager.GetQuestsByAdventureGuideCategory(QuestAdventureGuideCategory.WildHunt));
            questScheduleIds.UnionWith(QuestManager.GetQuestsByAdventureGuideCategory(QuestAdventureGuideCategory.AreaTrialOrMission));
            questScheduleIds.UnionWith(QuestManager.GetQuestsByAdventureGuideCategory(QuestAdventureGuideCategory.VocationQuest));
            questScheduleIds.UnionWith(QuestManager.GetQuestsByAdventureGuideCategory(QuestAdventureGuideCategory.QuestUsefulForAdventure));
            questScheduleIds.UnionWith(QuestManager.GetActiveQuestScheduleIds(client));

            var categoryOrder = new List<QuestAdventureGuideCategory>()
            {
                QuestAdventureGuideCategory.MainQuest,
                QuestAdventureGuideCategory.WildHunt,
                QuestAdventureGuideCategory.QuestUsefulForAdventure,
                QuestAdventureGuideCategory.VocationQuest,
                QuestAdventureGuideCategory.AreaTrialOrMission,
                QuestAdventureGuideCategory.CollaborationOrSeasonalQuest,
                QuestAdventureGuideCategory.LevelingMission,
                QuestAdventureGuideCategory.LevelingQuest,
            };

            foreach (var category in categoryOrder)
            {
                var matches = questScheduleIds
                    .Where(x => QuestManager.GetQuestByScheduleId(x).AdventureGuideCategory == category)
                    .OrderBy(x => QuestManager.GetQuestByScheduleId(x).BaseLevel)
                    .ToList();
                foreach (var questScheduleId in matches)
                {
                    var quest = QuestManager.GetQuestByScheduleId(questScheduleId);
                    if (quest == null || !quest.ShowInGuide(client) || client.Character.CompletedQuests.ContainsKey(quest.QuestId))
                    {
                        continue;
                    }

                    var questState = QuestManager.GetQuestStateManager(client, quest).GetQuestState(quest);

                    var step = questState?.Step ?? 0;
                    if (quest.QuestType == QuestType.World)
                    {
                        // World quests are identified by lestania news
                        // This feature seems to make that obsolete
                        // Defer world quests to lestania news for now
                        continue;
                    }

                    if (quest.AdventureGuideCategory == QuestAdventureGuideCategory.AreaTrialOrMission && 
                        !Server.AreaRankManager.PlayerCanParticipateInTrial(client, quest))
                    {
                        continue;
                    }
                    else if (quest.QuestType == QuestType.Tutorial && 
                            (quest.AdventureGuideCategory != QuestAdventureGuideCategory.CollaborationOrSeasonalQuest) &&
                            (quest.BaseLevel > client.Character.ActiveCharacterJobData.Lv) &&
                            (step == 0))
                    {
                        // Don't recommend quests that most likely can't be completed by the player
                        // at their current level
                        continue;
                    }

                    result.QuestList.Add(quest.ToCDataQuestAdventureGuideList(0));
                }
            }

            return result;
        }

        private bool IsQuestInLvRange(GameClient client, uint baseLevel)
        {
            uint plLv = client.Character.ActiveCharacterJobData.Lv;
            uint range = Server.GameSettings.GameServerSettings.AdventureGuideLevelRangeFilter;

            uint minLv = (plLv > (range + 1)) ? (plLv - range) : 1;
            uint maxLv = plLv + range;

            return (baseLevel >= minLv) && (baseLevel <= maxLv);
        }
    }
}
