using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CDispelGetDispelItemListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_DISPEL_GET_DISPEL_ITEM_LIST_RES;

        public S2CDispelGetDispelItemListRes()
        {
            DispelBaseItemList = new List<CDataDispelBaseItem>();
        }

        public List<CDataDispelBaseItem> DispelBaseItemList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CDispelGetDispelItemListRes>
        {
            public override void Write(IBuffer buffer, S2CDispelGetDispelItemListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.DispelBaseItemList);
            }

            public override S2CDispelGetDispelItemListRes Read(IBuffer buffer)
            {
                S2CDispelGetDispelItemListRes obj = new S2CDispelGetDispelItemListRes();
                ReadServerResponse(buffer, obj);
                obj.DispelBaseItemList = ReadEntityList<CDataDispelBaseItem>(buffer);
                return obj;
            }
        }
    }
}



