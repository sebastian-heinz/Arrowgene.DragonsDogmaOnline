using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Enemies.Generators
{
    public class CautionSpotEnemyGenerator : IEnemySetGenerator
    {
        public bool GetEnemySet(DdonGameServer server, GameClient client, StageLayoutId stageLayoutId, byte subGroupId, List<InstancedEnemy> instancedEnemySet, out QuestId questId)
        {
            questId = QuestId.WorldManageMonsterCaution;

            var matches = server.ScriptManager.MonsterCautionSpotModule.EnemyGroups.Values
                .SelectMany(x => x)
                .Where(x => x.GetLocation().Equals((stageLayoutId, subGroupId)))
                .OrderByDescending(x => x.RequiredAreaRank)
                .ToList();

            var cautionSpots = new List<IMonsterSpotInfo>();
            foreach (var match in matches)
            {
                var areaRank = server.AreaRankManager.GetEffectiveRank(client.Party.Leader.Client.Character, match.AreaId);
                if (areaRank >= match.RequiredAreaRank)
                {
                    cautionSpots.Add(match);
                }
            }
            
            if (cautionSpots.Count > 0)
            {
                instancedEnemySet.AddRange(cautionSpots[0].GetInstanceEnemies());
            }

            return instancedEnemySet.Count > 0;
        }
    }
}
