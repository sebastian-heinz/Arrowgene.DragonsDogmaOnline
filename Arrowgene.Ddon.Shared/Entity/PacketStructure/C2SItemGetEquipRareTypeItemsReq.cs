using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftGetEquipRareTypeItemsReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ITEM_GET_EQUIP_RARE_TYPE_ITEMS_REQ;

        public C2SCraftGetEquipRareTypeItemsReq()
        {
        }

        public byte Unk0 { get; set; } // Always 1?
        public byte Unk1 { get; set; } // Always 4?

        public class Serializer : PacketEntitySerializer<C2SCraftGetEquipRareTypeItemsReq>
        {
            public override void Write(IBuffer buffer, C2SCraftGetEquipRareTypeItemsReq obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
            }

            public override C2SCraftGetEquipRareTypeItemsReq Read(IBuffer buffer)
            {
                C2SCraftGetEquipRareTypeItemsReq obj = new C2SCraftGetEquipRareTypeItemsReq();
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadByte(buffer);
                return obj;
            }
        }

    }
}