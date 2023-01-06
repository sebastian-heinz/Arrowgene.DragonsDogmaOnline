using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SItemGetStorageItemListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ITEM_GET_STORAGE_ITEM_LIST_REQ;

        public C2SItemGetStorageItemListReq()
        {
            StorageList = new List<CDataCommonU8>();
        }

        public List<CDataCommonU8> StorageList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SItemGetStorageItemListReq>
        {
            public override void Write(IBuffer buffer, C2SItemGetStorageItemListReq obj)
            {
                WriteEntityList<CDataCommonU8>(buffer, obj.StorageList);
            }

            public override C2SItemGetStorageItemListReq Read(IBuffer buffer)
            {
                C2SItemGetStorageItemListReq obj = new C2SItemGetStorageItemListReq();
                obj.StorageList = ReadEntityList<CDataCommonU8>(buffer);
                return obj;
            }
        }

    }
}