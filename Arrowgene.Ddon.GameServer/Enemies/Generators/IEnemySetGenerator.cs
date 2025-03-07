using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Enemies.Generators
{
    public interface IEnemySetGenerator
    {
        public bool GetEnemySet(DdonGameServer server, GameClient client, StageLayoutId stageLayoutId, byte subGroupId, List<InstancedEnemy> instancedEnemySet, out QuestId questId);
    }
}
