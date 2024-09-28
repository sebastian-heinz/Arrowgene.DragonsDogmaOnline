using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Crypto;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
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
            StageLayoutId stageId = request.LayoutId.AsStageLayoutId();
            byte subGroupId = request.SubGroupId;
            client.Character.Stage = stageId;

            Logger.Info($"StageId={stageId}, SubGroupId={request.SubGroupId}");

            Quest quest = null;
            bool IsQuestControlled = false;
            foreach (var questScheduleId in QuestManager.CollectQuestScheduleIds(client, stageId))
            {
                quest = QuestManager.GetQuestByScheduleId(questScheduleId);

                var questStateManager = QuestManager.GetQuestStateManager(client, quest);
                if (quest.OverrideEnemySpawn && quest.HasEnemiesInCurrentStageGroup(stageId))
                {
                    IsQuestControlled = true;
                    break;
                }
                else if (!quest.OverrideEnemySpawn && questStateManager.HasEnemiesForCurrentQuestStepInStageGroup(quest, stageId, subGroupId))
                {
                    IsQuestControlled = true;
                    break;
                }
            }

            S2CInstanceGetEnemySetListRes response = new S2CInstanceGetEnemySetListRes()
            {
                LayoutId = stageId.ToCDataStageLayoutId(),
                SubGroupId = subGroupId,
                RandomSeed = CryptoRandom.Instance.GetRandomUInt32(),
            };

            List<InstancedEnemy> instancedEnemyList;

            bool notifyStrongEnemy = false;
            if (IsQuestControlled && quest != null)
            {
                response.QuestId = (uint) quest.QuestId;

                var questStateManager = QuestManager.GetQuestStateManager(client, quest);
                instancedEnemyList = questStateManager.GetInstancedEnemies(quest, stageId, subGroupId);
            }
            else if (Server.EpitaphRoadManager.TrialHasEnemies(client.Party, stageId, subGroupId))
            {
                instancedEnemyList = Server.EpitaphRoadManager.GetInstancedEnemies(client.Party, stageId, subGroupId);
            }
            else if (!client.Party.ExmInProgress)
            {
                instancedEnemyList = client.Party.InstanceEnemyManager.GetAssets(stageId).Where(x => x.Subgroup == subGroupId).Select(x => new InstancedEnemy(x)).ToList();
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
                var em = client.Party.InstanceEnemyManager.GetInstanceEnemy(stageId, enemy.Index);
                if (em == null)
                {
                    em = enemy;
                    // TODO: Add for HOBO dungeon
                    if (StageManager.IsEpitaphRoadStageId(stageId))
                    {
                        enemy.BloodOrbs = Server.EpitaphRoadManager.CalculateBloodOrbBonus(client.Party, em);
                    }
                    client.Party.InstanceEnemyManager.SetInstanceEnemy(stageId, em.Index, em);
                }
                em.StageLayoutId = stageId;

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
                    LayoutId = stageId.ToCDataStageLayoutId(),
                };

                client.Party.SendToAll(subgroupNtc);
            }

            return response;
        }
    }
}
