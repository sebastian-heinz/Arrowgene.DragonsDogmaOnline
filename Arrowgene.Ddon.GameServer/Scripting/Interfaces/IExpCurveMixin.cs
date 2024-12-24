using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Scripting.Interfaces
{
    public abstract class IExpCurveMixin
    {
        public abstract uint GetExpValue(InstancedEnemy enemy);
    }
}
