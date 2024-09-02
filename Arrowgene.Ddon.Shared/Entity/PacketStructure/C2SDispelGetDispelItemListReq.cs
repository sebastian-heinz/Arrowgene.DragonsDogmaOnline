using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SDispelGetDispelItemListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_DISPEL_GET_DISPEL_ITEM_LIST_REQ;

        public C2SDispelGetDispelItemListReq()
        {
        }

        public byte Category {  get; set; }

        public class Serializer : PacketEntitySerializer<C2SDispelGetDispelItemListReq>
        {
            public override void Write(IBuffer buffer, C2SDispelGetDispelItemListReq obj)
            {
                WriteByte(buffer, obj.Category);
            }

            public override C2SDispelGetDispelItemListReq Read(IBuffer buffer)
            {
                C2SDispelGetDispelItemListReq obj = new C2SDispelGetDispelItemListReq();
                obj.Category = ReadByte(buffer);
                return obj;
            }
        }

    }
}
