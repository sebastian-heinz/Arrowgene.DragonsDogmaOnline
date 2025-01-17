using System.Linq;
using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGoodsParam : ICloneable
    {
        public CDataGoodsParam() {
            Requirements = new();
        }
    
        // PS4 fields: Index, Price, Stock, MaxStock, RequireFavorite, ItemId (all uint)
        public uint Index { get; set; } // 0 based
        public uint ItemId { get; set; }
        public uint Price { get; set; }
        public byte Stock { get; set; } // 255 for unlimited
        public bool HideIfReqsUnmet { get; set; }
        public DateTimeOffset SalesPeriodStart { get; set; }
        public DateTimeOffset SalesPeriodEnd { get; set; }
        public List<CDataGoodsParamRequirement> Requirements { get; set; } // Requirements?

        public object Clone()
        {
            return new CDataGoodsParam()
            {
                Index = this.Index,
                ItemId = this.ItemId,
                Price = this.Price,
                Stock = this.Stock,
                HideIfReqsUnmet = this.HideIfReqsUnmet,
                SalesPeriodStart = this.SalesPeriodStart,
                SalesPeriodEnd = this.SalesPeriodEnd,
                Requirements = this.Requirements.Select(gpu7 => (CDataGoodsParamRequirement) gpu7.Clone()).ToList()
            };
        }
    
        public class Serializer : EntitySerializer<CDataGoodsParam>
        {
            public override void Write(IBuffer buffer, CDataGoodsParam obj)
            {
                WriteUInt32(buffer, obj.Index);
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt32(buffer, obj.Price);
                WriteByte(buffer, obj.Stock);
                WriteBool(buffer, obj.HideIfReqsUnmet);
                WriteInt64(buffer, obj.SalesPeriodStart.ToUnixTimeSeconds());
                WriteInt64(buffer, obj.SalesPeriodEnd.ToUnixTimeSeconds());
                WriteEntityList<CDataGoodsParamRequirement>(buffer, obj.Requirements);
            }
        
            public override CDataGoodsParam Read(IBuffer buffer)
            {
                CDataGoodsParam obj = new CDataGoodsParam();
                obj.Index = ReadUInt32(buffer);
                obj.ItemId = ReadUInt32(buffer);
                obj.Price = ReadUInt32(buffer);
                obj.Stock = ReadByte(buffer);
                obj.HideIfReqsUnmet = ReadBool(buffer);
                obj.SalesPeriodStart = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                obj.SalesPeriodEnd = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                obj.Requirements = ReadEntityList<CDataGoodsParamRequirement>(buffer);
                return obj;
            }
        }
    }
}
