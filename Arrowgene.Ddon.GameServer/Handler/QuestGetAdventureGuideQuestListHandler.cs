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

        private Dictionary<QuestAreaId,ContentsRelease> WorldQuestRequiredUnlocks = new Dictionary<QuestAreaId, ContentsRelease>()
        {
            // S2
            [QuestAreaId.BloodbaneIsle] = ContentsRelease.BloodbaneIsleWorldQuests,
            [QuestAreaId.ElanWaterGrove] = ContentsRelease.ElanWaterGroveWorldQuests,
            [QuestAreaId.FaranaPlains] = ContentsRelease.FaranaPlainsWorldQuests,
            [QuestAreaId.MorrowForest] = ContentsRelease.MorrowForestWorldQuests,
            [QuestAreaId.KingalCanyon] = ContentsRelease.KingalCanyonWorldQuests,
            // S3
            [QuestAreaId.RathniteFoothills] = ContentsRelease.RathniteFoothillsWorldQuests,
            [QuestAreaId.FeryanaWilderness] = ContentsRelease.FeryanaWildernessWorldQuests,
            [QuestAreaId.MegadosysPlateau] = ContentsRelease.MegadosysPlateauWorldQuests,
            [QuestAreaId.UrtecaMountains] = ContentsRelease.UrtecaMountainsWorldQuests,
        };

        public override S2CQuestGetAdventureGuideQuestListRes Handle(GameClient client, C2SQuestGetAdventureGuideQuestListReq request)
        {
            // var pcap0 = EntitySerializer.Get<S2CQuestGetAdventureGuideQuestListRes>().Read(GameFull.Dump_196.AsBuffer());
            var result = new S2CQuestGetAdventureGuideQuestListRes();

            // Check All Seasonal Events, Collab Events, Wild Hunts and active quests
            var questScheduleIds = QuestManager.GetQuestsByAdventureGuideCategory(QuestAdventureGuideCategory.CollaborationOrSeasonalQuest);
            questScheduleIds.UnionWith(QuestManager.GetQuestsByAdventureGuideCategory(QuestAdventureGuideCategory.WildHunt));
            questScheduleIds.UnionWith(QuestManager.GetQuestsByAdventureGuideCategory(QuestAdventureGuideCategory.AreaTrialOrMission));
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
                    if (quest == null || !quest.IsActive(client) || client.Character.CompletedQuests.ContainsKey(quest.QuestId))
                    {
                        continue;
                    }

                    var questState = QuestManager.GetQuestStateManager(client, quest).GetQuestState(quest);

                    var step = (questState == null) ? 0 : questState.Step;
                    if (quest.QuestType == QuestType.World)
                    {
                        // Skip the debug /warp quest
                        if ((uint)quest.QuestId == 70000001)
                        {
                            continue;
                        }

                        if (!client.Character.HasContentReleased(ContentsRelease.WorldQuests))
                        {
                            // Don't reccomend world quests until they get unlocked
                            continue;
                        }

                        if (WorldQuestRequiredUnlocks.ContainsKey(quest.QuestAreaId) &&
                            !client.Character.HasContentReleased(WorldQuestRequiredUnlocks[quest.QuestAreaId]))
                        {
                            // Don't show world quests in extended areas if they are not unlocked
                            continue;
                        }

                        if (!IsQuestInLvRange(client, quest.BaseLevel))
                        {
                            continue;
                        }

                        if (step == 0 && !quest.IsDiscoverable)
                        {
                            // Don't show hidden quests that are not started
                            continue;
                        }
                    }
                    else if (quest.AdventureGuideCategory == QuestAdventureGuideCategory.AreaTrialOrMission && 
                        !Server.AreaRankManager.PlayerCanParticipateInTrial(client, quest))
                    {
                        continue;
                    }
                    else if (quest.QuestType == QuestType.Tutorial && 
                            (quest.AdventureGuideCategory != QuestAdventureGuideCategory.CollaborationOrSeasonalQuest) &&
                            (quest.BaseLevel > client.Character.ActiveCharacterJobData.Lv) &&
                            (step == 0))
                    {
                        // Don't reccomend quests that most likely can't be completed by the player
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
