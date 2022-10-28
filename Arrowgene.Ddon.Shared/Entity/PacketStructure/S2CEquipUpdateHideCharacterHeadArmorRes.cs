using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEquipUpdateHideCharacterHeadArmorRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_EQUIP_UPDATE_HIDE_CHARACTER_HEAD_ARMOR_RES;

        public bool Hide { get; set; }

        public class Serializer : PacketEntitySerializer<S2CEquipUpdateHideCharacterHeadArmorRes>
        {
            public override void Write(IBuffer buffer, S2CEquipUpdateHideCharacterHeadArmorRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteBool(buffer, obj.Hide);
            }

            public override S2CEquipUpdateHideCharacterHeadArmorRes Read(IBuffer buffer)
            {
                S2CEquipUpdateHideCharacterHeadArmorRes obj = new S2CEquipUpdateHideCharacterHeadArmorRes();
                ReadServerResponse(buffer, obj);
                obj.Hide = ReadBool(buffer);
                return obj;
            }
        }
    }
}