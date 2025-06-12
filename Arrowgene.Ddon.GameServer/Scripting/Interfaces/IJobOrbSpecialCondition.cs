using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.GameServer.Scripting.Interfaces
{
    public abstract class IJobOrbSpecialCondition
    {
        public abstract uint ConditionId { get; }
        public abstract string Message { get; }
        public abstract bool EvaluateCondition(GameClient client);

        public CDataJobOrbDevoteElementSpecialCondition ToCDataJobOrbDevoteElementSpecialCondition(GameClient client)
        {
            return new()
            {
                ConditionId = ConditionId,
                Message = Message,
                IsReleased = EvaluateCondition(client)
            };
        }
    }
}
