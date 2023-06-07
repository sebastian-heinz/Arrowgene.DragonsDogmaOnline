using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SItemSortGetItemSortDataBinReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ITEM_SORT_GET_ITEM_SORTDATA_BIN_REQ;

        public List<CDataCommonU32> SortList { get; set; }

        public C2SItemSortGetItemSortDataBinReq()
        {
            SortList = new List<CDataCommonU32>();
        }

        public class Serializer : PacketEntitySerializer<C2SItemSortGetItemSortDataBinReq>
        {
            public override void Write(IBuffer buffer, C2SItemSortGetItemSortDataBinReq obj)
            {
                WriteEntityList<CDataCommonU32>(buffer, obj.SortList);
            }
        
            public override C2SItemSortGetItemSortDataBinReq Read(IBuffer buffer)
            {
                C2SItemSortGetItemSortDataBinReq obj = new C2SItemSortGetItemSortDataBinReq();
                obj.SortList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}