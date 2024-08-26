using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftEquipEnhancedEnhanceItemReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_EQUIP_ENHANCED_ENHANCE_ITEM_REQ;

        public C2SCraftEquipEnhancedEnhanceItemReq()
        {
            Unk1 = string.Empty;
            Unk2 = string.Empty;
            Unk3 = new CDataCraftEquipEnhancedEnhanceItemReqUnk0();
            Unk4 = new CDataCraftEquipEnhancedEnhanceItemReqUnk1();
        }

        public ushort Unk0 { get; set; } // Probably the Effect ID
        public string Unk1 { get; set; } // Probably ItemUID
        public string Unk2 { get; set; } // Probably Refinement material UID
        public CDataCraftEquipEnhancedEnhanceItemReqUnk0 Unk3 { get; set; }
        public CDataCraftEquipEnhancedEnhanceItemReqUnk1 Unk4 { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCraftEquipEnhancedEnhanceItemReq>
        {
            public override void Write(IBuffer buffer, C2SCraftEquipEnhancedEnhanceItemReq obj)
            {
                WriteUInt16(buffer, obj.Unk0);
                WriteMtString(buffer, obj.Unk1);
                WriteMtString(buffer, obj.Unk2);
                WriteEntity<CDataCraftEquipEnhancedEnhanceItemReqUnk0>(buffer, obj.Unk3);
                WriteEntity<CDataCraftEquipEnhancedEnhanceItemReqUnk1>(buffer, obj.Unk4);
            }

            public override C2SCraftEquipEnhancedEnhanceItemReq Read(IBuffer buffer)
            {
                C2SCraftEquipEnhancedEnhanceItemReq obj = new C2SCraftEquipEnhancedEnhanceItemReq();
                obj.Unk0 = ReadUInt16(buffer);
                obj.Unk1 = ReadMtString(buffer);
                obj.Unk2 = ReadMtString(buffer);
                obj.Unk3 = ReadEntity<CDataCraftEquipEnhancedEnhanceItemReqUnk0>(buffer);
                obj.Unk4 = ReadEntity<CDataCraftEquipEnhancedEnhanceItemReqUnk1>(buffer);
                return obj;
            }
        }

    }
}