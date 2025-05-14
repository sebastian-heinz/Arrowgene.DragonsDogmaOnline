using System;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class LightQuestRecord
    {
        public QuestId QuestId { get; set; }
        public LightQuestInfo QuestInfo { get
            {
                return LightQuestId.FromQuestId(QuestId);
            }
        }
        public uint QuestScheduleId { get
            {
                return Quest.QuestScheduleId.GenerateRotatingId(4, VariantIndex);
            }
        }
        public uint VariantIndex { get; set; }
        public int Target { get; set; }
        public int Count { get; set; }
        public ushort Level { get; set; }
        public uint RewardR {  get; set; }
        public uint RewardG { get; set; }
        public uint RewardXP { get; set; }
        public uint RewardAP { get; set; }
        public DateTimeOffset DistributionStart { get; set; }
        public DateTimeOffset DistributionEnd { get; set; }
    }
}
