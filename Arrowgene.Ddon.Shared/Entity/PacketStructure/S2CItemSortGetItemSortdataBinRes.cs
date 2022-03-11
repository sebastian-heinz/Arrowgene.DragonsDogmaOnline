using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CItemSortGetItemSortdataBinRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ITEM_SORT_GET_ITEM_SORTDATA_BIN_RES;

        public S2CItemSortGetItemSortdataBinRes()
        {
            SortData = new List<CDataItemSort>();
        }

        public List<CDataItemSort> SortData;

        public class Serializer : PacketEntitySerializer<S2CItemSortGetItemSortdataBinRes>
        {
            public override void Write(IBuffer buffer, S2CItemSortGetItemSortdataBinRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataItemSort>(buffer, obj.SortData);
            }

            public override S2CItemSortGetItemSortdataBinRes Read(IBuffer buffer)
            {
                S2CItemSortGetItemSortdataBinRes obj = new S2CItemSortGetItemSortdataBinRes();
                ReadServerResponse(buffer, obj);
                obj.SortData = ReadEntityList<CDataItemSort>(buffer);
                return obj;
            }
        }
    }
}