using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEquipUpdateHideCharacterHeadArmorReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_EQUIP_UPDATE_HIDE_CHARACTER_HEAD_ARMOR_REQ;

        public bool Hide { get; set; }

        public class Serializer : PacketEntitySerializer<C2SEquipUpdateHideCharacterHeadArmorReq>
        {
            public override void Write(IBuffer buffer, C2SEquipUpdateHideCharacterHeadArmorReq obj)
            {
                WriteBool(buffer, obj.Hide);
            }

            public override C2SEquipUpdateHideCharacterHeadArmorReq Read(IBuffer buffer)
            {
                C2SEquipUpdateHideCharacterHeadArmorReq obj = new C2SEquipUpdateHideCharacterHeadArmorReq();
                obj.Hide = ReadBool(buffer);
                return obj;
            }
        }
    }
}