using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEquipUpdateEquipPresetRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_EQUIP_UPDATE_EQUIP_PRESET_RES;

        public S2CEquipUpdateEquipPresetRes()
        {
            EquipPreset = new CDataEquipPreset();
        }

        public CDataEquipPreset EquipPreset { get; set; }

        public class Serializer : PacketEntitySerializer<S2CEquipUpdateEquipPresetRes>
        {
            public override void Write(IBuffer buffer, S2CEquipUpdateEquipPresetRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity(buffer, obj.EquipPreset);
            }

            public override S2CEquipUpdateEquipPresetRes Read(IBuffer buffer)
            {
                S2CEquipUpdateEquipPresetRes obj = new S2CEquipUpdateEquipPresetRes();
                ReadServerResponse(buffer, obj);
                obj.EquipPreset = ReadEntity<CDataEquipPreset>(buffer);
                return obj;
            }
        }
    }
}
