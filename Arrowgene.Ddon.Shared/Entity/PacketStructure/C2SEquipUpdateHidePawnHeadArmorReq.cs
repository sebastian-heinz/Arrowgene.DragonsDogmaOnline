using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEquipUpdateHidePawnHeadArmorReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_EQUIP_UPDATE_HIDE_PAWN_HEAD_ARMOR_REQ;

        public bool Hide { get; set; }

        public class Serializer : PacketEntitySerializer<C2SEquipUpdateHidePawnHeadArmorReq>
        {
            public override void Write(IBuffer buffer, C2SEquipUpdateHidePawnHeadArmorReq obj)
            {
                WriteBool(buffer, obj.Hide);
            }

            public override C2SEquipUpdateHidePawnHeadArmorReq Read(IBuffer buffer)
            {
                C2SEquipUpdateHidePawnHeadArmorReq obj = new C2SEquipUpdateHidePawnHeadArmorReq();
                obj.Hide = ReadBool(buffer);
                return obj;
            }
        }
    }
}