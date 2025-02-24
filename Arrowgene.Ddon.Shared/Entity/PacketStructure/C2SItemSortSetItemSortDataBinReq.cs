using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SItemSortSetItemSortDataBinReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ITEM_SORT_SET_ITEM_SORTDATA_BIN_REQ;

        public CDataItemSort SortData { get; set; } = new();

        public class Serializer : PacketEntitySerializer<C2SItemSortSetItemSortDataBinReq>
        {
            public override void Write(IBuffer buffer, C2SItemSortSetItemSortDataBinReq obj)
            {
                WriteEntity<CDataItemSort>(buffer, obj.SortData);
            }
        
            public override C2SItemSortSetItemSortDataBinReq Read(IBuffer buffer)
            {
                C2SItemSortSetItemSortDataBinReq obj = new C2SItemSortSetItemSortDataBinReq();
                obj.SortData = ReadEntity<CDataItemSort>(buffer);
                return obj;
            }
        }
    }
}
