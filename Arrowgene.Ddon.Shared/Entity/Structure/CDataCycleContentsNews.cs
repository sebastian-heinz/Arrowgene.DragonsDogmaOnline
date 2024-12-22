using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCycleContentsNews
    {
        public uint CycleContentsScheduleId { get; set; }
        public ulong Begin { get; set; }
        public ulong End { get; set; }
        public byte Category { get; set; }
        public uint CategoryType { get; set; }
        public List<CDataRewardItem> RewardItemList { get; set; }
        public List<CDataCycleContentsNewsDetail> DetailList { get; set; }
        public List<CDataCycleContentsRank> CycleContentsRankList { get; set; }
        public uint TotalPoint { get; set; }
        public uint PlayNum { get; set; }
        public bool IsCreateRanking { get; set; }

        public class Serializer : EntitySerializer<CDataCycleContentsNews>
        {
            public override void Write(IBuffer buffer, CDataCycleContentsNews obj)
            {
            }

            public override CDataCycleContentsNews Read(IBuffer buffer)
            {
                CDataCycleContentsNews obj = new CDataCycleContentsNews();
                return obj;
            }
        }
    }
}
