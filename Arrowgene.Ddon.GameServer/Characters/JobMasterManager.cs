using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class JobMasterManager
    {
        private DdonGameServer Server;

        public JobMasterManager(DdonGameServer server)
        {
            Server = server;
        }

        public PacketQueue HandleEnemyKill(GameClient client, InstancedEnemy enemy, DbConnection? connectionIn = null)
        {
            PacketQueue packets = new();

            var targetId = Enemy.GetNameId((EnemyId)enemy.EnemyId);
            if (targetId == 0)
            {
                // TODO: Should we log a message?
                // Enemy with no tracking killed
                return new();
            }

            bool isOrbEnemy = enemy.BloodOrbs > 0;

            foreach (var memberClient in client.Party.Clients)
            {
                var jobId = memberClient.Character.ActiveCharacterJobData.Job;
                if (!memberClient.Character.JobMasterActiveOrders.ContainsKey(jobId))
                {
                    // No Active job master requests, so skip
                    continue;
                }

                var matchingOrders = memberClient.Character.JobMasterActiveOrders[jobId]
                    .Where(x => x.JobOrderProgressList.Any(c => (c.TargetId == targetId) || ((c.ConditionType == JobOrderCondType.BloodOrbEnemies) && isOrbEnemy)))
                    .Where(x => x.OrderAccepted)
                    .ToList();
                if (matchingOrders.Count == 0)
                {
                    // No order for this enemy exists
                    continue;
                }

                foreach (var matchingOrder in matchingOrders)
                {
                    bool updatedRecord = false;
                    foreach (var orderProgress in matchingOrder.JobOrderProgressList)
                    {
                        if (orderProgress.CurrentNum >= orderProgress.TargetNum || enemy.Lv < orderProgress.TargetRank)
                        {
                            // Objective was completed, but not claimed
                            // or the enemy level was too low
                            continue;
                        }

                        updatedRecord = true;
                        orderProgress.CurrentNum += 1;

                        if (!Server.Database.UpsertJobMasterActiveOrdersProgress(memberClient.Character.CharacterId, jobId, matchingOrder.ReleaseType, matchingOrder.ReleaseId, orderProgress, connectionIn))
                        {
                            throw new ResponseErrorException(ErrorCode.ERROR_CODE_DB_FAILURE, "Failed to upsert job training record");
                        }
                    }

                    if (updatedRecord && matchingOrder.JobOrderProgressList.All(x => x.CurrentNum == x.TargetNum))
                    {
                        // Alert the player that they completed their training
                        packets.Enqueue(memberClient, new S2CJobOrderCompleteNtc()
                        {
                            JobId = jobId,
                            RewardLv = matchingOrder.ReleaseLv,
                            RewardNo = matchingOrder.ReleaseId,
                            RewardType = matchingOrder.ReleaseType
                        });
                    }
                }
            }

            return packets;
        }

        public List<CDataCommonU32> GetNewOrders(GameClient client, JobId jobId, DbConnection? connectionIn = null)
        {
            var newOrders = client.Character.JobMasterActiveOrders
                .Where(x => x.Key == jobId)
                .SelectMany(x => x.Value)
                .Where(x => !x.OrderAccepted)
                .ToList();

            foreach (var order in newOrders)
            {
                order.OrderAccepted = true;
                Server.Database.UpsertJobMasterActiveOrder(client.Character.CharacterId, jobId, order, connectionIn);
            }

            return newOrders.Select(x => new CDataCommonU32(x.ReleaseId)).ToList();
        }

        public List<CDataReleaseElement> GetReleasedElements(GameClient client, JobId jobId)
        {
            return client.Character.JobMasterReleasedElements[jobId];
        }

        public List<CDataReleaseElement> GetNewReleasedElements(GameClient client, JobId jobId, DbConnection? connectionIn = null)
        {
            var completedOrders = client.Character.JobMasterActiveOrders[jobId]
                    .Where(x => x.JobOrderProgressList.All(x => x.TargetNum == x.CurrentNum)).ToList();

            var newReleasedElement = new List<CDataReleaseElement>();
            foreach (var completedOrder in completedOrders)
            {
                var releasedElement = new CDataReleaseElement()
                {
                    ReleaseId = completedOrder.ReleaseId,
                    ReleaseLv = completedOrder.ReleaseLv,
                    ReleaseType = completedOrder.ReleaseType,
                };

                Server.Database.DeleteJobMasterActiveOrder(client.Character.CharacterId, jobId, completedOrder, connectionIn);
                Server.Database.InsertJobMasterReleasedElement(client.Character.CharacterId, jobId, releasedElement, connectionIn);

                newReleasedElement.Add(releasedElement);

                // Add to internal state
                client.Character.JobMasterReleasedElements[jobId].Add(releasedElement);

                // Update existing skill
                if (completedOrder.ReleaseType == JobTrainingReleaseType.CustomSkill)
                {
                    var acquireableSkill = client.Character.AcquirableSkills[jobId]
                        .Where(x => x.SkillNo == releasedElement.ReleaseId)
                        .SelectMany(x => x.Params)
                        .Where(x => x.Lv == releasedElement.ReleaseLv)
                        .FirstOrDefault();
                    acquireableSkill.IsRelease = true;
                }
                else 
                {
                    var acquireableSkill = client.Character.AcquirableAbilities[jobId]
                        .Where(x => x.AbilityNo == releasedElement.ReleaseId)
                        .SelectMany(x => x.Params)
                        .Where(x => x.Lv == releasedElement.ReleaseLv)
                        .FirstOrDefault();
                    acquireableSkill.IsRelease = true;
                }

                // Remove tracking for completed order
                client.Character.JobMasterActiveOrders[jobId].RemoveAll(x => x.ReleaseId == completedOrder.ReleaseId);
            }

            return newReleasedElement;
        }

        public void ScheduleCustomSkillTrainingTask(GameClient client, JobId jobId, CustomSkill customSkill, DbConnection? connectionIn = null)
        {
            var match = Server.AssetRepository.JobMasterAsset.JobOrders[jobId][JobTrainingReleaseType.CustomSkill]
                .SelectMany(x => x.Value)
                .Where(x => x.ReleaseLv == customSkill.SkillLv + 1 && x.ReleaseId == customSkill.SkillId)
                .FirstOrDefault();
            if (match == null)
            {
                return;
            }

            // Create the new order
            Server.Database.InsertJobMasterActiveOrder(client.Character.CharacterId, jobId, match, connectionIn);
            foreach (var condition in match.JobOrderProgressList)
            {
                Server.Database.InsertJobMasterActiveOrderProgress(client.Character.CharacterId, jobId, JobTrainingReleaseType.CustomSkill, match.ReleaseId, condition, connectionIn);
            }

            // Update the stored orders
            client.Character.JobMasterActiveOrders[jobId] = GetJobMasterActiveOrders(client.Character, jobId, connectionIn);
        }

        public void ScheduleAbilityTrainingTask(GameClient client, JobId jobId, Ability ability, DbConnection? connectionIn = null)
        {
            var match = Server.AssetRepository.JobMasterAsset.JobOrders[jobId][JobTrainingReleaseType.Augment]
                .SelectMany(x => x.Value)
                .Where(x => x.ReleaseLv == ability.AbilityLv + 1 && x.ReleaseId == ability.AbilityId)
                .FirstOrDefault();
            if (match == null)
            {
                return;
            }

            // Create the new order
            Server.Database.InsertJobMasterActiveOrder(client.Character.CharacterId, jobId, match, connectionIn);
            foreach (var condition in match.JobOrderProgressList)
            {
                Server.Database.InsertJobMasterActiveOrderProgress(client.Character.CharacterId, jobId, JobTrainingReleaseType.Augment, match.ReleaseId, condition, connectionIn);
            }

            // Update the stored orders
            client.Character.JobMasterActiveOrders[jobId] = GetJobMasterActiveOrders(client.Character, jobId, connectionIn);
        }

        public List<CDataActiveJobOrder> GetJobMasterActiveOrders(Character character, JobId jobId, DbConnection? connectionIn)
        {
            var results = Server.Database.GetJobMasterActiveOrders(character.CharacterId, jobId, connectionIn);
            foreach (var activeOrder in results)
            {
                activeOrder.JobOrderProgressList = Server.Database.GetJobMasterActiveOrderProgress(character.CharacterId, jobId, activeOrder.ReleaseType, activeOrder.ReleaseId, connectionIn);
            }
            return results;
        }
    }
}
