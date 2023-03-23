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
            Unk7 = new List<CDataGoodsParamUnk7>();
        }
    
        // PS4 fields: Index, Price, Stock, MaxStock, RequireFavorite, ItemId (all uint)
        public uint Index { get; set; } // 0 based
        public uint ItemId { get; set; }
        public uint Price { get; set; }
        public byte Stock { get; set; } // 255 for unlimited
        public bool Unk4{ get; set; }
        public ulong Unk5 { get; set; }
        public ulong Unk6 { get; set; }
        public List<CDataGoodsParamUnk7> Unk7 { get; set; } // Requirements?

        public object Clone()
        {
            return new CDataGoodsParam()
            {
                Index = this.Index,
                ItemId = this.ItemId,
                Price = this.Price,
                Stock = this.Stock,
                Unk4 = this.Unk4,
                Unk5 = this.Unk5,
                Unk6 = this.Unk6,
                Unk7 = this.Unk7.Select(gpu7 => (CDataGoodsParamUnk7) gpu7.Clone()).ToList()
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
                WriteBool(buffer, obj.Unk4);
                WriteUInt64(buffer, obj.Unk5);
                WriteUInt64(buffer, obj.Unk6);
                WriteEntityList<CDataGoodsParamUnk7>(buffer, obj.Unk7);
            }
        
            public override CDataGoodsParam Read(IBuffer buffer)
            {
                CDataGoodsParam obj = new CDataGoodsParam();
                obj.Index = ReadUInt32(buffer);
                obj.ItemId = ReadUInt32(buffer);
                obj.Price = ReadUInt32(buffer);
                obj.Stock = ReadByte(buffer);
                obj.Unk4 = ReadBool(buffer);
                obj.Unk5 = ReadUInt64(buffer);
                obj.Unk6 = ReadUInt64(buffer);
                obj.Unk7 = ReadEntityList<CDataGoodsParamUnk7>(buffer);
                return obj;
            }
        }
    }
}