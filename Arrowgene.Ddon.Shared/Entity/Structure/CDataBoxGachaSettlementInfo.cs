using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBoxGachaSettlementInfo
    {
        public uint DrawId { get; set; }
        public uint Id { get; set; }
        public uint Price { get; set; }
        public uint BasePrice { get; set; }
        public byte DrawNum { get; set; }
        public byte BonusNum { get; set; }
        public uint PurchaseNum { get; set; }
        public uint PurchaseMaxNum { get; set; }
        public uint SpecialPriceNum { get; set; }
        public uint SpecialPriceMaxNum { get; set; }
        public uint Unk1 { get; set; }

        public CDataBoxGachaSettlementInfo()
        {
        }

        public class Serializer : EntitySerializer<CDataBoxGachaSettlementInfo>
        {
            public override void Write(IBuffer buffer, CDataBoxGachaSettlementInfo obj)
            {
                WriteUInt32(buffer, obj.DrawId);
                WriteUInt32(buffer, obj.Id);
                WriteUInt32(buffer, obj.Price);
                WriteUInt32(buffer, obj.BasePrice);
                WriteUInt32(buffer, obj.PurchaseNum);
                WriteByte(buffer, obj.DrawNum);
                WriteByte(buffer, obj.BonusNum);
                WriteUInt32(buffer, obj.PurchaseMaxNum);
                WriteUInt32(buffer, obj.SpecialPriceNum);
                WriteUInt32(buffer, obj.SpecialPriceMaxNum);
                WriteUInt32(buffer, obj.Unk1);
            }

            public override CDataBoxGachaSettlementInfo Read(IBuffer buffer)
            {
                CDataBoxGachaSettlementInfo obj = new CDataBoxGachaSettlementInfo
                {
                    DrawId = ReadUInt32(buffer),
                    Id = ReadUInt32(buffer),
                    Price = ReadUInt32(buffer),
                    BasePrice = ReadUInt32(buffer),
                    DrawNum = ReadByte(buffer),
                    BonusNum = ReadByte(buffer),
                    PurchaseNum = ReadUInt32(buffer),
                    PurchaseMaxNum = ReadUInt32(buffer),
                    SpecialPriceNum = ReadUInt32(buffer),
                    SpecialPriceMaxNum = ReadUInt32(buffer),
                    Unk1 = ReadUInt32(buffer)
                };

                return obj;
            }
        }
    }
}
