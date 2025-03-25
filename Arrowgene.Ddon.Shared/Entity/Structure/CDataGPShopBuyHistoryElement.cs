using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGPShopBuyHistoryElement
    {
        public uint ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public uint Price { get; set; }
        public ulong AcquisitionTime { get; set; }

        public CDataGPShopBuyHistoryElement()
        {
        }

        public class Serializer : EntitySerializer<CDataGPShopBuyHistoryElement>
        {
            public override void Write(IBuffer buffer, CDataGPShopBuyHistoryElement obj)
            {
                WriteUInt32(buffer, obj.ID);
                WriteMtString(buffer, obj.Name);
                WriteUInt32(buffer, obj.Price);
                WriteUInt64(buffer, obj.AcquisitionTime);
            }

            public override CDataGPShopBuyHistoryElement Read(IBuffer buffer)
            {
                CDataGPShopBuyHistoryElement obj = new CDataGPShopBuyHistoryElement
                {
                    ID = ReadUInt32(buffer),
                    Name = ReadMtString(buffer),
                    Price = ReadUInt32(buffer),
                    AcquisitionTime = ReadUInt64(buffer),
                };

                return obj;
            }
        }
    }
}
