using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SItemChangeAttrDiscardReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ITEM_CHANGE_ATTR_DISCARD_REQ;

        public C2SItemChangeAttrDiscardReq()
        {
            ItemList = new List<CDataItemUIDList>();
        }

        public List<CDataItemUIDList> ItemList { get; set; }
        public bool DiscardSetting { get; set; }

        public class Serializer : PacketEntitySerializer<C2SItemChangeAttrDiscardReq>
        {
            public override void Write(IBuffer buffer, C2SItemChangeAttrDiscardReq obj)
            {
                WriteEntityList(buffer, obj.ItemList);
                WriteBool(buffer, obj.DiscardSetting);
            }

            public override C2SItemChangeAttrDiscardReq Read(IBuffer buffer)
            {
                C2SItemChangeAttrDiscardReq obj = new C2SItemChangeAttrDiscardReq();
                obj.ItemList = ReadEntityList<CDataItemUIDList>(buffer);
                obj.DiscardSetting = ReadBool(buffer);
                return obj;
            }
        }
    }
}
