using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class QuestOrderCondition
    {
        public QuestOrderConditionType Type { get; set; }
        public int Param01 { get; set; }
        public int Param02 { get; set; }

        public CDataQuestOrderConditionParam ToCDataQuestOrderConditionParam()
        {
            return new CDataQuestOrderConditionParam()
            {
                Type = (uint)Type,
                Param01 = Param01,
                Param02 = Param02
            };
        }
    }
}
