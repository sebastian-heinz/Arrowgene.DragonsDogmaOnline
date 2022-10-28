using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEquipUpdateHidePawnHeadArmorRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_EQUIP_UPDATE_HIDE_PAWN_HEAD_ARMOR_RES;

        public bool Hide { get; set; }

        public class Serializer : PacketEntitySerializer<S2CEquipUpdateHidePawnHeadArmorRes>
        {
            public override void Write(IBuffer buffer, S2CEquipUpdateHidePawnHeadArmorRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteBool(buffer, obj.Hide);
            }

            public override S2CEquipUpdateHidePawnHeadArmorRes Read(IBuffer buffer)
            {
                S2CEquipUpdateHidePawnHeadArmorRes obj = new S2CEquipUpdateHidePawnHeadArmorRes();
                ReadServerResponse(buffer, obj);
                obj.Hide = ReadBool(buffer);
                return obj;
            }
        }
    }
}