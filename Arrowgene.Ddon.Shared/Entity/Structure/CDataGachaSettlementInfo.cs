using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGachaSettlementInfo
    {
        public uint DrawGroupId { get; set; }

        // Golden Gemstone = 1, Silver Tickets = 2
        public uint Id { get; set; }
        public uint Price { get; set; }
        public uint BasePrice { get; set; }
        public uint PurchaseNum { get; set; }
        public uint PurchaseMaxNum { get; set; }
        public uint SpecialPriceNum { get; set; }
        public uint SpecialPriceMaxNum { get; set; }
        public uint Unk1 { get; set; }

        public CDataGachaSettlementInfo()
        {
        }

        public class Serializer : EntitySerializer<CDataGachaSettlementInfo>
        {
            public override void Write(IBuffer buffer, CDataGachaSettlementInfo obj)
            {
                WriteUInt32(buffer, obj.DrawGroupId);
                WriteUInt32(buffer, obj.Id);
                WriteUInt32(buffer, obj.Price);
                WriteUInt32(buffer, obj.BasePrice);
                WriteUInt32(buffer, obj.PurchaseNum);
                WriteUInt32(buffer, obj.PurchaseMaxNum);
                WriteUInt32(buffer, obj.SpecialPriceNum);
                WriteUInt32(buffer, obj.SpecialPriceMaxNum);
                WriteUInt32(buffer, obj.Unk1);
            }

            public override CDataGachaSettlementInfo Read(IBuffer buffer)
            {
                CDataGachaSettlementInfo obj = new CDataGachaSettlementInfo
                {
                    DrawGroupId = ReadUInt32(buffer),
                    Id = ReadUInt32(buffer),
                    Price = ReadUInt32(buffer),
                    BasePrice = ReadUInt32(buffer),
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
