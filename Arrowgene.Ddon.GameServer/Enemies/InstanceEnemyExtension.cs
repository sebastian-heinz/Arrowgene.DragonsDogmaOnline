using Arrowgene.Ddon.GameServer.Scripting;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Enemies
{
    public static class InstancedEnemyExtensions
    {
        public static InstancedEnemy SetNamedEnemyParams(this InstancedEnemy enemy, uint namedParamId)
        {
            return enemy.SetNamedEnemyParams(LibDdon.Enemy.GetNamedParam(namedParamId));
        }
    }
}
