using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataTimeGainQuestUnk2
    {
        public CDataTimeGainQuestUnk2()
        {
            WalletPointList = new List<CDataWalletPoint>();
            Unk0List = new List<CDataS2CEquipEnhancedGetPacksResUnk0Unk6>();
        }

        public List<CDataWalletPoint> WalletPointList { get; set; }

        public List<CDataS2CEquipEnhancedGetPacksResUnk0Unk6> Unk0List { get; set;}

        public class Serializer : EntitySerializer<CDataTimeGainQuestUnk2>
        {
            public override void Write(IBuffer buffer, CDataTimeGainQuestUnk2 obj)
            {
                WriteEntityList(buffer, obj.WalletPointList);
                WriteEntityList(buffer, obj.Unk0List);
            }

            public override CDataTimeGainQuestUnk2 Read(IBuffer buffer)
            {
                CDataTimeGainQuestUnk2 obj = new CDataTimeGainQuestUnk2();
                obj.WalletPointList = ReadEntityList<CDataWalletPoint>(buffer);
                obj.Unk0List = ReadEntityList<CDataS2CEquipEnhancedGetPacksResUnk0Unk6>(buffer);
                return obj;
            }
        }
    }
}
