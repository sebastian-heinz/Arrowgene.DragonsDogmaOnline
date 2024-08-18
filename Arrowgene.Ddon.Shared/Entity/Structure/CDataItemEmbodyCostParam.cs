using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataItemEmbodyCostParam
    {
        public CDataItemEmbodyCostParam()
        {
            WalletPoints = new List<CDataWalletPoint>();
            Unk1 = new List<CDataS2CEquipEnhancedGetPacksResUnk0Unk6>();
        }

        public uint Unk0 { get; set; } // Total Currency?
        public List<CDataWalletPoint> WalletPoints { get; set; }
        public List<CDataS2CEquipEnhancedGetPacksResUnk0Unk6> Unk1 {  get; set; }

        public class Serializer : EntitySerializer<CDataItemEmbodyCostParam>
        {
            public override void Write(IBuffer buffer, CDataItemEmbodyCostParam obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteEntityList<CDataWalletPoint>(buffer, obj.WalletPoints);
                WriteEntityList<CDataS2CEquipEnhancedGetPacksResUnk0Unk6>(buffer, obj.Unk1);
            }

            public override CDataItemEmbodyCostParam Read(IBuffer buffer)
            {
                CDataItemEmbodyCostParam obj = new CDataItemEmbodyCostParam();
                obj.Unk0 = ReadUInt32(buffer);
                obj.WalletPoints = ReadEntityList<CDataWalletPoint>(buffer);
                obj.Unk1 = ReadEntityList<CDataS2CEquipEnhancedGetPacksResUnk0Unk6>(buffer);
                return obj;
            }
        }
    }
}
