using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SItemGetItemStorageInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ITEM_GET_ITEM_STORAGE_INFO_REQ;

        public List<CDataGameItemStorage> GameItemStorageList;

        public C2SItemGetItemStorageInfoReq()
        {
            GameItemStorageList = new List<CDataGameItemStorage>();
        }

        public class Serializer : PacketEntitySerializer<C2SItemGetItemStorageInfoReq>
        {
            public override void Write(IBuffer buffer, C2SItemGetItemStorageInfoReq obj)
            {
                WriteEntityList<CDataGameItemStorage>(buffer, obj.GameItemStorageList);
            }

            public override C2SItemGetItemStorageInfoReq Read(IBuffer buffer)
            {
                C2SItemGetItemStorageInfoReq obj = new C2SItemGetItemStorageInfoReq();

                obj.GameItemStorageList = ReadEntityList<CDataGameItemStorage>(buffer);

                return obj;
            }
        }
    }
}
