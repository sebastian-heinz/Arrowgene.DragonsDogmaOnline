using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Crypto;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp.Consumer.BlockingQueueConsumption;
using static Arrowgene.Ddon.Server.Network.Challenge;

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
            QuestState questState = null;
            bool IsQuestControlled = false;
            foreach (var questId in client.Party.QuestState.StageQuests(stageId))
            {
                quest = QuestManager.GetQuest(questId);
                questState = client.Party.QuestState.GetQuestState(questId);

                if (quest.HasEnemiesInCurrentStageGroup(questState, stageId, subGroupId))
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

            if (IsQuestControlled && quest != null)
            {
                response.QuestId = (uint) quest.QuestId;
                response.EnemyList = client.Party.QuestState.GetInstancedEnemies(quest.QuestId, stageId, subGroupId).Select(enemy => new CDataLayoutEnemyData()
                {
                    PositionIndex = (byte)(enemy.Index),
                    EnemyInfo = enemy.asCDataStageLayoutEnemyPresetEnemyInfoClient()
                }).ToList();
            }
            else
            {
                response.EnemyList = client.Party.InstanceEnemyManager.GetAssets(stageId, subGroupId).Select((enemy, index) => new CDataLayoutEnemyData()
                {
                    PositionIndex = (byte)index,
                    EnemyInfo = enemy.asCDataStageLayoutEnemyPresetEnemyInfoClient()
                })
                .ToList();
            }

            client.Send(response);
        }
    }
}
