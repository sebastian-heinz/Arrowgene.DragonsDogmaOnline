using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CItemEmbodyItemsRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ITEM_EMBODY_ITEMS_RES;

        public S2CItemEmbodyItemsRes()
        {
            Unk0 = new List<CDataStorageEmptySlotNum>();
            Unk1 = new List<CDataS2CEquipEnhancedGetPacksResUnk0Unk6>();
            WalletPoints = new List<CDataWalletPoint>();
            Unk2 = new List<CDataS2CEquipEnhancedGetPacksResUnk0Unk6>();
        }

        public List<CDataStorageEmptySlotNum> Unk0 { get; set; }
        public List<CDataS2CEquipEnhancedGetPacksResUnk0Unk6> Unk1 { get; set; } // This is not the same type but it has same size elements
        public List<CDataWalletPoint> WalletPoints { get; set; }
        public List<CDataS2CEquipEnhancedGetPacksResUnk0Unk6> Unk2 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CItemEmbodyItemsRes>
        {
            public override void Write(IBuffer buffer, S2CItemEmbodyItemsRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.Unk0);
                WriteEntityList(buffer, obj.Unk1);
                WriteEntityList(buffer, obj.WalletPoints);
                WriteEntityList(buffer, obj.Unk2);
            }

            public override S2CItemEmbodyItemsRes Read(IBuffer buffer)
            {
                S2CItemEmbodyItemsRes obj = new S2CItemEmbodyItemsRes();
                ReadServerResponse(buffer, obj);
                obj.Unk0 = ReadEntityList<CDataStorageEmptySlotNum>(buffer);
                obj.Unk1 = ReadEntityList<CDataS2CEquipEnhancedGetPacksResUnk0Unk6>(buffer);
                obj.WalletPoints = ReadEntityList<CDataWalletPoint>(buffer);
                obj.Unk2 = ReadEntityList<CDataS2CEquipEnhancedGetPacksResUnk0Unk6>(buffer);
                return obj;
            }
        }
    }
}
