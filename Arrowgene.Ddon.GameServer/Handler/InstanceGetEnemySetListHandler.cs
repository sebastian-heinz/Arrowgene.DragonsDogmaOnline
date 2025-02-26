using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Crypto;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetEnemySetListHandler : GameRequestPacketHandler<C2SInstanceGetEnemySetListReq, S2CInstanceGetEnemySetListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetEnemySetListHandler));

        public InstanceGetEnemySetListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CInstanceGetEnemySetListRes Handle(GameClient client, C2SInstanceGetEnemySetListReq request)
        {
            StageLayoutId stageLayoutId = request.LayoutId.AsStageLayoutId();
            byte subGroupId = request.SubGroupId;
            client.Character.Stage = stageLayoutId;

            Logger.Info($"StageId={stageLayoutId}, SubGroupId={request.SubGroupId}");

            Quest quest = FindQuestBasedOnPriority(client, stageLayoutId, subGroupId);
            bool isQuestControlled = (quest != null);

            S2CInstanceGetEnemySetListRes response = new S2CInstanceGetEnemySetListRes()
            {
                LayoutId = stageLayoutId.ToCDataStageLayoutId(),
                SubGroupId = subGroupId,
                RandomSeed = CryptoRandom.Instance.GetRandomUInt32(),
            };

            List<InstancedEnemy> instancedEnemyList;

            bool notifyStrongEnemy = false;
            if (isQuestControlled && quest != null)
            {
                response.QuestId = (uint) quest.QuestId;

                var questStateManager = QuestManager.GetQuestStateManager(client, quest);
                instancedEnemyList = questStateManager.GetInstancedEnemies(quest, stageLayoutId, subGroupId);
            }
            else if (Server.EpitaphRoadManager.TrialHasEnemies(client.Party, stageLayoutId, subGroupId))
            {
                instancedEnemyList = Server.EpitaphRoadManager.GetInstancedEnemies(client.Party, stageLayoutId, subGroupId);
            }
            else if (!client.Party.ExmInProgress)
            {
                instancedEnemyList = client.Party.InstanceEnemyManager.GetAssets(stageLayoutId).Where(x => x.Subgroup == subGroupId).Select(x => new InstancedEnemy(x)).ToList();
            }
            else
            {
                // This can happen in EXM where we don't want to return any
                // monsters which might exist outside the quest spawns.
                // Example, EXM which takes place in BBI, this is the normal
                // BBI map, not a special one allocated for the EXM.
                instancedEnemyList = new();
            }

            foreach (var enemy in instancedEnemyList)
            {
                var em = client.Party.InstanceEnemyManager.GetInstanceEnemy(stageLayoutId, enemy.Index);
                if (em == null)
                {
                    em = enemy;
                    // TODO: Add for HOBO dungeon
                    if (StageManager.IsEpitaphRoadStageId(stageLayoutId))
                    {
                        enemy.BloodOrbs = Server.EpitaphRoadManager.CalculateBloodOrbBonus(client.Party, em);
                    }
                    client.Party.InstanceEnemyManager.SetInstanceEnemy(stageLayoutId, em.Index, em);
                }
                em.StageLayoutId = stageLayoutId;

                response.EnemyList.Add(new CDataLayoutEnemyData()
                {
                    PositionIndex = em.Index,
                    EnemyInfo = em.asCDataStageLayoutEnemyPresetEnemyInfoClient()
                });

                if (em.NotifyStrongEnemy)
                {
                    notifyStrongEnemy = true;
                }
            }

            if (notifyStrongEnemy)
            {
                // TODO: Send NTC which creates popup
            }

            if (subGroupId > 0 && response.EnemyList.Count > 0)
            {
                S2CInstanceEnemySubGroupAppearNtc subgroupNtc = new S2CInstanceEnemySubGroupAppearNtc()
                {
                    SubGroupId = subGroupId,
                    LayoutId = stageLayoutId.ToCDataStageLayoutId(),
                };

                client.Party.SendToAll(subgroupNtc);
            }

            return response;
        }

        private Quest FindQuestBasedOnPriority(GameClient client, StageLayoutId stageLayoutId, uint subgroupId)
        {

            var quests = new List<Quest>();
            foreach (var questScheduleId in QuestManager.CollectQuestScheduleIds(client, stageLayoutId))
            {
                var quest = QuestManager.GetQuestByScheduleId(questScheduleId);
                var questStateManager = QuestManager.GetQuestStateManager(client, quest);
                if (quest.OverrideEnemySpawn && quest.HasEnemiesInCurrentStageGroup(stageLayoutId))
                {
                    quests.Add(quest);
                }
                else if (!quest.OverrideEnemySpawn && questStateManager.HasEnemiesForCurrentQuestStepInStageGroup(quest, stageLayoutId, subgroupId))
                {
                    quests.Add(quest);
                }
            }

            // There may be multiple quests conflicting for a StageId.LayerNo.GroupNo.
            // Certain quests should have a higher priority than other quests
            // and this list describes the ranking of the different quest types.
            var questPriorityList = new List<QuestType>()
            {
                QuestType.Main,
                QuestType.Tutorial,
                QuestType.WildHunt,
                QuestType.World,
            };

            Quest priorityQuest = null;
            foreach (var questType in questPriorityList)
            {
                var matches = quests.Where(x => x.QuestType == questType).ToList();
                if (matches.Count > 0)
                {
                    priorityQuest = matches[0];
                    break;
                }
            }

            if (priorityQuest == null && quests.Count > 0)
            {
                priorityQuest = quests[0];
            }

            return priorityQuest;
        }
    }
}
