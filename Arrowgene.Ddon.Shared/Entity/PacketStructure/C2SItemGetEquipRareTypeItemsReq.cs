using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SItemGetEquipRareTypeItemsReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ITEM_GET_EQUIP_RARE_TYPE_ITEMS_REQ;

        public C2SItemGetEquipRareTypeItemsReq()
        {
        }

        public byte Unk0 { get; set; }
        public byte Unk1 { get; set; }

        public class Serializer : PacketEntitySerializer<C2SItemGetEquipRareTypeItemsReq>
        {
            public override void Write(IBuffer buffer, C2SItemGetEquipRareTypeItemsReq obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
            }

            public override C2SItemGetEquipRareTypeItemsReq Read(IBuffer buffer)
            {
                C2SItemGetEquipRareTypeItemsReq obj = new C2SItemGetEquipRareTypeItemsReq();
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadByte(buffer);
                return obj;
            }
        }
    }
}
