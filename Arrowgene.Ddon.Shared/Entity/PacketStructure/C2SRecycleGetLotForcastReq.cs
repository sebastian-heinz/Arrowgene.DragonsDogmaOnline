using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SRecycleGetLotForcastReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_RECYCLE_GET_LOT_FORECAST_REQ;

        public C2SRecycleGetLotForcastReq()
        {
            ItemUID = string.Empty;
        }

        public string ItemUID { get; set; }

        public class Serializer : PacketEntitySerializer<C2SRecycleGetLotForcastReq>
        {
            public override void Write(IBuffer buffer, C2SRecycleGetLotForcastReq obj)
            {
                WriteMtString(buffer, obj.ItemUID);
            }

            public override C2SRecycleGetLotForcastReq Read(IBuffer buffer)
            {
                C2SRecycleGetLotForcastReq obj = new C2SRecycleGetLotForcastReq();
                obj.ItemUID = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
