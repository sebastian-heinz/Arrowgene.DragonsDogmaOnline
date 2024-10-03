using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
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

        public WalletType WalletType { get; set; } // Not sure if accurate but what it is being used for.
        public List<CDataWalletPoint> WalletPoints { get; set; }
        public List<CDataS2CEquipEnhancedGetPacksResUnk0Unk6> Unk1 {  get; set; }

        public class Serializer : EntitySerializer<CDataItemEmbodyCostParam>
        {
            public override void Write(IBuffer buffer, CDataItemEmbodyCostParam obj)
            {
                WriteUInt32(buffer, (uint) obj.WalletType);
                WriteEntityList<CDataWalletPoint>(buffer, obj.WalletPoints);
                WriteEntityList<CDataS2CEquipEnhancedGetPacksResUnk0Unk6>(buffer, obj.Unk1);
            }

            public override CDataItemEmbodyCostParam Read(IBuffer buffer)
            {
                CDataItemEmbodyCostParam obj = new CDataItemEmbodyCostParam();
                obj.WalletType = (WalletType) ReadUInt32(buffer);
                obj.WalletPoints = ReadEntityList<CDataWalletPoint>(buffer);
                obj.Unk1 = ReadEntityList<CDataS2CEquipEnhancedGetPacksResUnk0Unk6>(buffer);
                return obj;
            }
        }
    }
}
