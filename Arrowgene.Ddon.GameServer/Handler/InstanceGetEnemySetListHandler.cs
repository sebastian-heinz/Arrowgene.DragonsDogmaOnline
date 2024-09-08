using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Crypto;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetEnemySetListHandler : StructurePacketHandler<GameClient, C2SInstanceGetEnemySetListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetEnemySetListHandler));

        public InstanceGetEnemySetListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceGetEnemySetListReq> request)
        {
            StageId stageId = StageId.FromStageLayoutId(request.Structure.LayoutId);
            byte subGroupId = request.Structure.SubGroupId;
            client.Character.Stage = stageId;

            Logger.Info($"GroupId={request.Structure.LayoutId.GroupId}, SubGroupId={request.Structure.SubGroupId}, LayerNo={request.Structure.LayoutId.LayerNo}");

            Quest quest = null;
            bool IsQuestControlled = false;
            foreach (var questId in client.Party.QuestState.StageQuests(stageId))
            {
                quest = client.Party.QuestState.GetQuest(questId);
                if (client.Party.QuestState.HasEnemiesInCurrentStageGroup(quest, stageId, subGroupId))
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

            bool notifyStrongEnemy = false;
            if (IsQuestControlled && quest != null)
            {
                response.QuestId = (uint) quest.QuestId;

                foreach (var enemy in client.Party.QuestState.GetInstancedEnemies(quest, stageId, subGroupId))
                {
                    response.EnemyList.Add(new CDataLayoutEnemyData()
                    {
                        PositionIndex = enemy.Index,
                        EnemyInfo = enemy.asCDataStageLayoutEnemyPresetEnemyInfoClient()
                    });
                    client.Party.InstanceEnemyManager.SetInstanceEnemy(stageId, enemy.Index, enemy);

                    if (enemy.NotifyStrongEnemy)
                    {
                        notifyStrongEnemy = true;
                    }
                }
            }
            else
            {
                foreach (var asset in client.Party.InstanceEnemyManager.GetAssets(stageId, 0)
                    .Where(x => x.Subgroup == subGroupId))
                {
                    response.EnemyList.Add(new CDataLayoutEnemyData()
                    {
                        PositionIndex = asset.Index,
                        EnemyInfo = asset.asCDataStageLayoutEnemyPresetEnemyInfoClient()
                    });
                    client.Party.InstanceEnemyManager.SetInstanceEnemy(stageId, asset.Index, asset);

                    if (asset.NotifyStrongEnemy)
                    {
                        notifyStrongEnemy = true;
                    }
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
