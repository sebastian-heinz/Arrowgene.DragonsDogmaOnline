using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SRecycleStartExchangeReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_RECYCLE_START_EXCHANGE_REQ;

        public C2SRecycleStartExchangeReq()
        {
            ItemUID = string.Empty;
        }

        public byte StorageType { get; set; }
        public string ItemUID { get; set; }

        public class Serializer : PacketEntitySerializer<C2SRecycleStartExchangeReq>
        {
            public override void Write(IBuffer buffer, C2SRecycleStartExchangeReq obj)
            {
                WriteByte(buffer, obj.StorageType);
                WriteMtString(buffer, obj.ItemUID);
            }

            public override C2SRecycleStartExchangeReq Read(IBuffer buffer)
            {
                C2SRecycleStartExchangeReq obj = new C2SRecycleStartExchangeReq();
                obj.StorageType = ReadByte(buffer);
                obj.ItemUID = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
