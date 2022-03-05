using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CItemSortGetItemSortdataBinNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_ITEM_SORT_GET_ITEM_SORTDATA_BIN_NTC;

        public S2CItemSortGetItemSortdataBinNtc()
        {
            SortData = new List<CDataItemSort>();
        }

        public List<CDataItemSort> SortData;

        public class Serializer : PacketEntitySerializer<S2CItemSortGetItemSortdataBinNtc>
        {
            public override void Write(IBuffer buffer, S2CItemSortGetItemSortdataBinNtc obj)
            {
                WriteEntityList<CDataItemSort>(buffer, obj.SortData);
            }

            public override S2CItemSortGetItemSortdataBinNtc Read(IBuffer buffer)
            {
                S2CItemSortGetItemSortdataBinNtc obj = new S2CItemSortGetItemSortdataBinNtc();
                obj.SortData = ReadEntityList<CDataItemSort>(buffer);
                return obj;
            }
        }
    }
}