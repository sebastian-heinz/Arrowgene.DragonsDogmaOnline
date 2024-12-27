using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CItemGetItemStorageInfoRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ITEM_GET_ITEM_STORAGE_INFO_RES;

        public List<CDataGameItemStorageInfo> GameItemStorageInfoList { get; set; }

        public S2CItemGetItemStorageInfoRes()
        {
            GameItemStorageInfoList = new List<CDataGameItemStorageInfo>();
        }

        public class Serializer : PacketEntitySerializer<S2CItemGetItemStorageInfoRes>
        {
            public override void Write(IBuffer buffer, S2CItemGetItemStorageInfoRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteEntityList<CDataGameItemStorageInfo>(buffer, obj.GameItemStorageInfoList);
            }

            public override S2CItemGetItemStorageInfoRes Read(IBuffer buffer)
            {
                S2CItemGetItemStorageInfoRes obj = new S2CItemGetItemStorageInfoRes();

                ReadServerResponse(buffer, obj);

                obj.GameItemStorageInfoList = ReadEntityList<CDataGameItemStorageInfo>(buffer);

                return obj;
            }
        }
    }
}
