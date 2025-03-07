using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Scripting.Interfaces
{
    public abstract class IInstanceEnemyPropertyGenerator
    {
        public abstract void ApplyChanges(GameClient client, StageLayoutId stageLayotuId, byte subGroupId, InstancedEnemy enemy);
    }
}
