using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEquipEnhancedEnhanceItemReq : IPacketStructure
    {
        public C2SEquipEnhancedEnhanceItemReq()
        {
            UpgradeItemUID = string.Empty;
            Unk2 = string.Empty;
            UpgradeWalletCost = new List<CDataWalletPoint>();
            UpgradeItemCostList = new List<CDataEquipEnhanceItem>();
        }

        public PacketId Id => PacketId.C2S_EQUIP_ENHANCED_ENHANCE_ITEM_REQ;

        public ushort CategoryIndex { get; set; }
        public string UpgradeItemUID { get; set; }
        public string Unk2 { get; set; }
        public List<CDataWalletPoint> UpgradeWalletCost { get; set; }
        public List<CDataEquipEnhanceItem> UpgradeItemCostList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SEquipEnhancedEnhanceItemReq>
        {
            public override void Write(IBuffer buffer, C2SEquipEnhancedEnhanceItemReq obj)
            {
                WriteUInt16(buffer, obj.CategoryIndex);
                WriteMtString(buffer, obj.UpgradeItemUID);
                WriteMtString(buffer, obj.Unk2);
                WriteEntityList(buffer, obj.UpgradeWalletCost);
                WriteEntityList(buffer, obj.UpgradeItemCostList);
            }

            public override C2SEquipEnhancedEnhanceItemReq Read(IBuffer buffer)
            {
                C2SEquipEnhancedEnhanceItemReq obj = new C2SEquipEnhancedEnhanceItemReq();
                obj.CategoryIndex = ReadUInt16(buffer);
                obj.UpgradeItemUID = ReadMtString(buffer);
                obj.Unk2 = ReadMtString(buffer);
                obj.UpgradeWalletCost = ReadEntityList<CDataWalletPoint>(buffer);
                obj.UpgradeItemCostList = ReadEntityList<CDataEquipEnhanceItem>(buffer);
                return obj;
            }
        }

    }
}
