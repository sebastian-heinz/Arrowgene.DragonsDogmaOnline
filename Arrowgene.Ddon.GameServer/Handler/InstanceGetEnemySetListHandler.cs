using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Crypto;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetEnemySetListHandler : StructurePacketHandler<GameClient, C2SInstanceGetEnemySetListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetEnemySetListHandler));

        private DdonGameServer _Server;

        public InstanceGetEnemySetListHandler(DdonGameServer server) : base(server)
        {
            _Server = server;
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceGetEnemySetListReq> request)
        {
            StageId stageId = StageId.FromStageLayoutId(request.Structure.LayoutId);
            byte subGroupId = request.Structure.SubGroupId;
            client.Character.Stage = stageId;

            Logger.Info($"GroupId={request.Structure.LayoutId.GroupId}, SubGroupId={request.Structure.SubGroupId}, LayerNo={request.Structure.LayoutId.LayerNo}");

            Quest quest = null;
            bool IsQuestControlled = false;
            foreach (var questScheduleId in QuestManager.CollectQuestScheduleIds(client, stageId))
            {
                quest = QuestManager.GetQuestByScheduleId(questScheduleId);

                var questStateManager = QuestManager.GetQuestStateManager(client, quest);
                if (quest.OverrideEnemySpawn && quest.HasEnemiesInInCurrentStage(stageId))
                {
                    IsQuestControlled = true;
                    break;
                }
                else if (!quest.OverrideEnemySpawn && questStateManager.HasEnemiesInCurrentStageGroup(quest, stageId, subGroupId))
                {
                    IsQuestControlled = true;
                    break;
                }
            }

            S2CInstanceGetEnemySetListRes response = new S2CInstanceGetEnemySetListRes()
            {
                LayoutId = stageId.ToStageLayoutId(),
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
            else if (_Server.EpitaphRoadManager.TrialHasEnemies(client.Party, stageId, subGroupId))
            {
                instancedEnemyList = _Server.EpitaphRoadManager.GetInstancedEnemies(client.Party, stageId, subGroupId);
            }
            else
            {
                instancedEnemyList = client.Party.InstanceEnemyManager.GetAssets(stageId).Where(x => x.Subgroup == subGroupId).Select(x => new InstancedEnemy(x)).ToList();
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
                        enemy.BloodOrbs = _Server.EpitaphRoadManager.CalculateBloodOrbBonus(client.Party, em);
                    }
                    client.Party.InstanceEnemyManager.SetInstanceEnemy(stageId, em.Index, em);
                }

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
                    LayoutId = stageId.ToStageLayoutId(),
                };

                client.Party.SendToAll(subgroupNtc);
            }

            client.Send(response);
        }
    }
}
