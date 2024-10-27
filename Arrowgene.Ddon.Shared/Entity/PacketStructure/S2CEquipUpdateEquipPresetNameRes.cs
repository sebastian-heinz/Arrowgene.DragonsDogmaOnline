using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEquipUpdateEquipPresetNameRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_EQUIP_UPDATE_EQUIP_PRESET_NAME_RES;

        public S2CEquipUpdateEquipPresetNameRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CEquipUpdateEquipPresetNameRes>
        {
            public override void Write(IBuffer buffer, S2CEquipUpdateEquipPresetNameRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CEquipUpdateEquipPresetNameRes Read(IBuffer buffer)
            {
                S2CEquipUpdateEquipPresetNameRes obj = new S2CEquipUpdateEquipPresetNameRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
