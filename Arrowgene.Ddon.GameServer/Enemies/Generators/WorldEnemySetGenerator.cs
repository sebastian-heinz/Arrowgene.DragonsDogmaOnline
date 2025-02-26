using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Enemies.Generators
{
    public class WorldEnemySetGenerator : IEnemySetGenerator
    {
        public bool GetEnemySet(DdonGameServer server, GameClient client, StageLayoutId stageLayoutId, byte subGroupId, List<InstancedEnemy> instancedEnemySet, out QuestId questId)
        {
            questId = QuestId.None;
            if (client.Party.ExmInProgress)
            {
                return false;
            }

            StageInfo stageInfo = Stage.StageInfoFromStageLayoutId(stageLayoutId);
            var areaRank = server.AreaRankManager.GetEffectiveRank(client.Party.Leader.Client.Character, stageInfo);
            instancedEnemySet.AddRange(client.Party.InstanceEnemyManager.GetAssets(stageLayoutId)
                .Where(x => x.Subgroup == subGroupId)
                .Select(x => new InstancedEnemy(x))
                .Where(x => areaRank >= x.RequiredAreaRank)
                .ToList());

            return true;
        }
    }
}
