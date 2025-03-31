using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SItemUseJobItemsReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ITEM_USE_JOB_ITEMS_REQ;

        public C2SItemUseJobItemsReq()
        {
            ItemUIdList = new List<CDataItemUIDList>();
        }

        public List<CDataItemUIDList> ItemUIdList { get; set; }
        

        public class Serializer : PacketEntitySerializer<C2SItemUseJobItemsReq>
        {
            public override void Write(IBuffer buffer, C2SItemUseJobItemsReq obj)
            {
                WriteEntityList<CDataItemUIDList>(buffer, obj.ItemUIdList);
            }

            public override C2SItemUseJobItemsReq Read(IBuffer buffer)
            {
                C2SItemUseJobItemsReq obj = new();
                obj.ItemUIdList = ReadEntityList<CDataItemUIDList>(buffer);
                return obj;
            }
        }

    }
}