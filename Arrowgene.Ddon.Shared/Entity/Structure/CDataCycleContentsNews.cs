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
        public List<CDataRewardItem> RewardItemList { get; set; } = [];
        public List<CDataCycleContentsNewsDetail> DetailList { get; set; } = [];
        public List<CDataCycleContentsRank> CycleContentsRankList { get; set; } = [];
        public uint TotalPoint { get; set; }
        public uint PlayNum { get; set; }
        public bool IsCreateRanking { get; set; }
        public List<CDataQuestEnemyInfo> EnemyInfo { get; set; } = [];
        public List<CDataCycleContentsUnk> Unk0 { get; set; } = [];
        public DateTimeOffset UnkOffset0 { get; set; }
        public DateTimeOffset UnkOffset1 { get; set; }
        public DateTimeOffset UnkOffset2 { get; set; }
        public DateTimeOffset UnkOffset3 { get; set; }
        public DateTimeOffset UnkOffset4 { get; set; }
        public DateTimeOffset UnkOffset5 { get; set; }

        public class Serializer : EntitySerializer<CDataCycleContentsNews>
        {
            public override void Write(IBuffer buffer, CDataCycleContentsNews obj)
            {
                WriteUInt32(buffer, obj.CycleContentsScheduleId);
                WriteInt64(buffer, obj.Begin.ToUnixTimeSeconds());
                WriteInt64(buffer, obj.End.ToUnixTimeSeconds());
                WriteByte(buffer, obj.Category);
                WriteUInt32(buffer, obj.CategoryType);
                WriteEntityList(buffer, obj.RewardItemList);
                WriteEntityList(buffer, obj.DetailList);
                WriteEntityList(buffer, obj.CycleContentsRankList);
                WriteUInt32(buffer, obj.TotalPoint);
                WriteUInt32(buffer, obj.PlayNum);
                WriteBool(buffer, obj.IsCreateRanking);
                WriteEntityList(buffer, obj.EnemyInfo);
                WriteEntityList(buffer, obj.Unk0);
                WriteInt64(buffer, obj.UnkOffset0.ToUnixTimeSeconds());
                WriteInt64(buffer, obj.UnkOffset1.ToUnixTimeSeconds());
                WriteInt64(buffer, obj.UnkOffset2.ToUnixTimeSeconds());
                WriteInt64(buffer, obj.UnkOffset3.ToUnixTimeSeconds());
                WriteInt64(buffer, obj.UnkOffset4.ToUnixTimeSeconds());
                WriteInt64(buffer, obj.UnkOffset5.ToUnixTimeSeconds());
            }

            public override CDataCycleContentsNews Read(IBuffer buffer)
            {
                CDataCycleContentsNews obj = new CDataCycleContentsNews();
                obj.CycleContentsScheduleId = ReadUInt32(buffer);
                obj.Begin = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                obj.End = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                obj.Category = ReadByte(buffer);
                obj.CategoryType = ReadUInt32(buffer);
                obj.RewardItemList = ReadEntityList<CDataRewardItem>(buffer);
                obj.DetailList = ReadEntityList<CDataCycleContentsNewsDetail>(buffer);
                obj.CycleContentsRankList = ReadEntityList<CDataCycleContentsRank>(buffer);
                obj.TotalPoint = ReadUInt32(buffer);
                obj.PlayNum = ReadUInt32(buffer);
                obj.IsCreateRanking = ReadBool(buffer);
                obj.EnemyInfo = ReadEntityList<CDataQuestEnemyInfo>(buffer);
                obj.Unk0 = ReadEntityList<CDataCycleContentsUnk>(buffer);
                obj.UnkOffset0 = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                obj.UnkOffset1 = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                obj.UnkOffset2 = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                obj.UnkOffset3 = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                obj.UnkOffset4 = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                obj.UnkOffset5 = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                return obj;
            }
        }
    }
}
