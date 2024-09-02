using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEquipEnhancedGetPacksRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_EQUIP_ENHANCED_GET_PACKS_RES;

        public S2CEquipEnhancedGetPacksRes()
        {
            Unk0 = new List<CDataS2CEquipEnhancedGetPacksResUnk0>();
        }

        public List<CDataS2CEquipEnhancedGetPacksResUnk0> Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CEquipEnhancedGetPacksRes>
        {
            public override void Write(IBuffer buffer, S2CEquipEnhancedGetPacksRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataS2CEquipEnhancedGetPacksResUnk0>(buffer, obj.Unk0);
            }

            public override S2CEquipEnhancedGetPacksRes Read(IBuffer buffer)
            {
                S2CEquipEnhancedGetPacksRes obj = new S2CEquipEnhancedGetPacksRes();
                ReadServerResponse(buffer, obj);
                obj.Unk0 = ReadEntityList<CDataS2CEquipEnhancedGetPacksResUnk0>(buffer);
                return obj;
            }
        }
    }
}