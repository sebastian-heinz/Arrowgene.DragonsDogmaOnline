using System;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGoodsParamRequirement : ICloneable
    {    
        public uint Index { get; set; } // Maybe?
        public ShopItemUnlockCondition Condition { get; set; }
        public bool IgnoreRequirements { get; set; }
        public uint Progress { get; set; }
        public bool HideRequirementDetails { get; set; }
        public uint Param1 { get; set; }
        public uint Param2 { get; set; }
        public uint Param3 { get; set; }
        public uint Param4 { get; set; }
        public uint Param5 { get; set; }
        public DateTimeOffset SalesPeriodStart { get; set; }
        public DateTimeOffset SalesPeriodEnd { get; set; }

        public object Clone()
        {
            return new CDataGoodsParamRequirement()
            {
                Index = this.Index,
                Condition = this.Condition,
                IgnoreRequirements = this.IgnoreRequirements,
                Progress = this.Progress,
                HideRequirementDetails = this.HideRequirementDetails,
                Param1 = this.Param1,
                Param2 = this.Param2,
                Param3 = this.Param3,
                Param4 = this.Param4,
                Param5 = this.Param5,
                SalesPeriodStart = this.SalesPeriodStart,
                SalesPeriodEnd = this.SalesPeriodEnd
            };
        }

        public class Serializer : EntitySerializer<CDataGoodsParamRequirement>
        {
            public override void Write(IBuffer buffer, CDataGoodsParamRequirement obj)
            {
                WriteUInt32(buffer, obj.Index);
                WriteUInt32(buffer, (uint)obj.Condition);
                WriteBool(buffer, obj.IgnoreRequirements);
                WriteUInt32(buffer, obj.Progress);
                WriteBool(buffer, obj.HideRequirementDetails);
                WriteUInt32(buffer, obj.Param1);
                WriteUInt32(buffer, obj.Param2);
                WriteUInt32(buffer, obj.Param3);
                WriteUInt32(buffer, obj.Param4);
                WriteUInt32(buffer, obj.Param5);
                WriteInt64(buffer, obj.SalesPeriodStart.ToUnixTimeSeconds());
                WriteInt64(buffer, obj.SalesPeriodEnd.ToUnixTimeSeconds());
            }

            public override CDataGoodsParamRequirement Read(IBuffer buffer)
            {
                CDataGoodsParamRequirement obj = new CDataGoodsParamRequirement();
                obj.Index = ReadUInt32(buffer);
                obj.Condition = (ShopItemUnlockCondition)ReadUInt32(buffer);
                obj.IgnoreRequirements = ReadBool(buffer);
                obj.Progress = ReadUInt32(buffer);
                obj.HideRequirementDetails = ReadBool(buffer);
                obj.Param1 = ReadUInt32(buffer);
                obj.Param2 = ReadUInt32(buffer);
                obj.Param3 = ReadUInt32(buffer);
                obj.Param4 = ReadUInt32(buffer);
                obj.Param5 = ReadUInt32(buffer);
                obj.SalesPeriodStart = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                obj.SalesPeriodEnd = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                return obj;
            }
        }
    }
}
