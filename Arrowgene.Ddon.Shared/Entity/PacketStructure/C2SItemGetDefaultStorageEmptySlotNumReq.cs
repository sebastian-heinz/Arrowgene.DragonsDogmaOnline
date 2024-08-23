using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SItemGetDefaultStorageEmptySlotNumReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ITEM_GET_DEFAULT_STORAGE_EMPTY_SLOT_NUM_REQ;

        public C2SItemGetDefaultStorageEmptySlotNumReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SItemGetDefaultStorageEmptySlotNumReq>
        {
            public override void Write(IBuffer buffer, C2SItemGetDefaultStorageEmptySlotNumReq obj)
            {
            }

            public override C2SItemGetDefaultStorageEmptySlotNumReq Read(IBuffer buffer)
            {
                C2SItemGetDefaultStorageEmptySlotNumReq obj = new C2SItemGetDefaultStorageEmptySlotNumReq();
                return obj;
            }
        }

    }
}
