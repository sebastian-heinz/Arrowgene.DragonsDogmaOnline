using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCraftEquipEnhancedEnhanceItemRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_EQUIP_ENHANCED_ENHANCE_ITEM_RES;

        public S2CCraftEquipEnhancedEnhanceItemRes()
        {
            CurrentEquipInfo = new CDataCurrentEquipInfo();
        }

        public CDataCurrentEquipInfo CurrentEquipInfo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCraftEquipEnhancedEnhanceItemRes>
        {
            public override void Write(IBuffer buffer, S2CCraftEquipEnhancedEnhanceItemRes obj)
            {
                WriteEntity<CDataCurrentEquipInfo>(buffer, obj.CurrentEquipInfo);
            }

            public override S2CCraftEquipEnhancedEnhanceItemRes Read(IBuffer buffer)
            {
                S2CCraftEquipEnhancedEnhanceItemRes obj = new S2CCraftEquipEnhancedEnhanceItemRes();
                obj.CurrentEquipInfo = ReadEntity<CDataCurrentEquipInfo>(buffer);
                return obj;
            }
        }

    }
}