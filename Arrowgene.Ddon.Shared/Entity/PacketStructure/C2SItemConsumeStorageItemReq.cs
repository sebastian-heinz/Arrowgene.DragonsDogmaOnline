using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SItemConsumeStorageItemReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ITEM_CONSUME_STORAGE_ITEM_REQ;

        public C2SItemConsumeStorageItemReq()
        {
            ConsumeItemList = new List<CDataStorageItemUIDList>();
        }

        public List<CDataStorageItemUIDList> ConsumeItemList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SItemConsumeStorageItemReq>
        {
            public override void Write(IBuffer buffer, C2SItemConsumeStorageItemReq obj)
            {
                WriteEntityList<CDataStorageItemUIDList>(buffer, obj.ConsumeItemList);
            }

            public override C2SItemConsumeStorageItemReq Read(IBuffer buffer)
            {
                C2SItemConsumeStorageItemReq obj = new C2SItemConsumeStorageItemReq();
                obj.ConsumeItemList = ReadEntityList<CDataStorageItemUIDList>(buffer);
                return obj;
            }
        }

    }
}