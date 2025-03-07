using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Scripting.Interfaces
{
    public abstract class IExpMixin
    {
        public abstract uint GetExpValue(CharacterCommon characterCommon, InstancedEnemy enemy);
    }
}
