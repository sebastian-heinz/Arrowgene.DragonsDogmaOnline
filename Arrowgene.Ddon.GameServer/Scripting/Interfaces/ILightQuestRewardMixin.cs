using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.GameServer.Scripting.Interfaces
{
    public abstract class ILightQuestRewardMixin
    {
        public abstract uint CalculateRewardXP(LightQuestRecord record, double difficulty = 0.0);
        public abstract uint CalculateRewardR(LightQuestRecord record, double difficulty = 0.0);
        public abstract uint CalculateRewardG(LightQuestRecord record, double difficulty = 0.0);
        public abstract uint CalculateRewardAP(LightQuestRecord record, double difficulty = 0.0);
    }
}
