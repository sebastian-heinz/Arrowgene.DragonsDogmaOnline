using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEquipGetEquipPresetListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_EQUIP_GET_EQUIP_PRESET_LIST_RES;

        public S2CEquipGetEquipPresetListRes()
        {
            EquipPresetList = new List<CDataEquipPreset>();
        }

        public List<CDataEquipPreset> EquipPresetList;

        public class Serializer : PacketEntitySerializer<S2CEquipGetEquipPresetListRes>
        {
            public override void Write(IBuffer buffer, S2CEquipGetEquipPresetListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.EquipPresetList);
            }

            public override S2CEquipGetEquipPresetListRes Read(IBuffer buffer)
            {
                S2CEquipGetEquipPresetListRes obj = new S2CEquipGetEquipPresetListRes();
                ReadServerResponse(buffer, obj);
                obj.EquipPresetList = ReadEntityList<CDataEquipPreset>(buffer);
                return obj;
            }
        }
    }
}
