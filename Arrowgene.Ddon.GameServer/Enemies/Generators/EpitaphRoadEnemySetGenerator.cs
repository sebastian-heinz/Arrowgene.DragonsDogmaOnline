using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Enemies.Generators
{
    public class EpitaphRoadEnemySetGenerator : IEnemySetGenerator
    {
        public bool GetEnemySet(DdonGameServer server, GameClient client, StageLayoutId stageLayoutId, byte subGroupId, List<InstancedEnemy> instancedEnemySet, out QuestId questId)
        {
            questId = QuestId.None;
            if (!server.EpitaphRoadManager.TrialHasEnemies(client.Party, stageLayoutId, subGroupId))
            {
                return false;
            }

            instancedEnemySet.AddRange(server.EpitaphRoadManager.GetInstancedEnemies(client.Party, stageLayoutId, subGroupId));

            return true;
        }
    }
}
