using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.GameServer.Scripting.Interfaces
{
    public abstract class IPointModifier
    {
        public abstract PointType PointType { get; }
        public abstract PointModifierType ModifierType { get; }
        public abstract PointModifierAction ModifierAction { get; }
        public abstract RewardSource Source { get; }
        public abstract PlayerType PlayerTypes { get; }

        public virtual bool IsEnabled()
        {
            return true;
        }

        public abstract double GetMultiplier(GameMode gameMode, CharacterCommon characterCommon, PartyGroup party, InstancedEnemy enemy, QuestType questType = QuestType.All);
    }
}
