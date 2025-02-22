using Arrowgene.Buffers;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCycleContentsNews
    {
        public uint CycleContentsScheduleId { get; set; }
        public DateTimeOffset Begin { get; set; }
        public DateTimeOffset End { get; set; }
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
                WriteUInt32(buffer, obj.CycleContentsScheduleId);
                WriteUInt64(buffer, (ulong)obj.Begin.ToUnixTimeSeconds());
                WriteUInt64(buffer, (ulong)obj.End.ToUnixTimeSeconds());
                WriteByte(buffer, obj.Category);
                WriteUInt32(buffer, obj.CategoryType);
                WriteEntityList(buffer, obj.RewardItemList);
                WriteEntityList(buffer, obj.DetailList);
                WriteEntityList(buffer, obj.CycleContentsRankList);
                WriteUInt32(buffer, obj.TotalPoint);
                WriteUInt32(buffer, obj.PlayNum);
                WriteBool(buffer, obj.IsCreateRanking);
            }

            public override CDataCycleContentsNews Read(IBuffer buffer)
            {
                CDataCycleContentsNews obj = new CDataCycleContentsNews();
                obj.CycleContentsScheduleId = ReadUInt32(buffer);
                obj.Begin = DateTimeOffset.FromUnixTimeSeconds((long)ReadUInt64(buffer));
                obj.End = DateTimeOffset.FromUnixTimeSeconds((long)ReadUInt64(buffer));
                obj.Category = ReadByte(buffer);
                obj.CategoryType = ReadUInt32(buffer);
                obj.RewardItemList = ReadEntityList<CDataRewardItem>(buffer);
                obj.DetailList = ReadEntityList<CDataCycleContentsNewsDetail>(buffer);
                obj.CycleContentsRankList = ReadEntityList<CDataCycleContentsRank>(buffer);
                obj.TotalPoint = ReadUInt32(buffer);
                obj.PlayNum = ReadUInt32(buffer);
                obj.IsCreateRanking = ReadBool(buffer);
                return obj;
            }
        }
    }
}
