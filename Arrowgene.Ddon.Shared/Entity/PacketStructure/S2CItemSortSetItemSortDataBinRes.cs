using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CItemSortSetItemSortDataBinRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ITEM_SORT_SET_ITEM_SORTDATA_BIN_RES;

        public class Serializer : PacketEntitySerializer<S2CItemSortSetItemSortDataBinRes>
        {
            public override void Write(IBuffer buffer, S2CItemSortSetItemSortDataBinRes obj)
            {
                WriteServerResponse(buffer, obj);
            }
        
            public override S2CItemSortSetItemSortDataBinRes Read(IBuffer buffer)
            {
                S2CItemSortSetItemSortDataBinRes obj = new S2CItemSortSetItemSortDataBinRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}