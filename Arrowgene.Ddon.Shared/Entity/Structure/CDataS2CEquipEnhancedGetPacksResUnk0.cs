using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataS2CEquipEnhancedGetPacksResUnk0
    {
        public CDataS2CEquipEnhancedGetPacksResUnk0() {
            Unk2 = string.Empty;
            Unk5 = new List<CDataWalletPoint>();
            Unk6 = new List<CDataS2CEquipEnhancedGetPacksResUnk0Unk6>();
            Unk7 = new List<CDataWalletPoint>();
            Unk8 = new List<CDataS2CEquipEnhancedGetPacksResUnk0Unk6>();
            Unk9 = new List<CDataS2CEquipEnhancedGetPacksResUnk0Unk9>();
            Unk10 = new List<CDataS2CEquipEnhancedGetPacksResUnk0Unk10>();
        }
    
        public ushort Unk0 { get; set; }
        public byte Unk1 { get; set; }
        public string Unk2 { get; set; }
        public ushort Unk3 { get; set; }
        public ushort Unk4 { get; set; }
        public List<CDataWalletPoint> Unk5 { get; set; }
        public List<CDataS2CEquipEnhancedGetPacksResUnk0Unk6> Unk6 { get; set; }
        public List<CDataWalletPoint> Unk7 { get; set; }
        public List<CDataS2CEquipEnhancedGetPacksResUnk0Unk6> Unk8 { get; set; }
        public List<CDataS2CEquipEnhancedGetPacksResUnk0Unk9> Unk9 { get; set; }
        public List<CDataS2CEquipEnhancedGetPacksResUnk0Unk10> Unk10 { get; set; }
    
        public class Serializer : EntitySerializer<CDataS2CEquipEnhancedGetPacksResUnk0>
        {
            public override void Write(IBuffer buffer, CDataS2CEquipEnhancedGetPacksResUnk0 obj)
            {
                WriteUInt16(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
                WriteMtString(buffer, obj.Unk2);
                WriteUInt16(buffer, obj.Unk3);
                WriteUInt16(buffer, obj.Unk4);
                WriteEntityList<CDataWalletPoint>(buffer, obj.Unk5);
                WriteEntityList<CDataS2CEquipEnhancedGetPacksResUnk0Unk6>(buffer, obj.Unk6);
                WriteEntityList<CDataWalletPoint>(buffer, obj.Unk7);
                WriteEntityList<CDataS2CEquipEnhancedGetPacksResUnk0Unk6>(buffer, obj.Unk8);
                WriteEntityList<CDataS2CEquipEnhancedGetPacksResUnk0Unk9>(buffer, obj.Unk9);
                WriteEntityList<CDataS2CEquipEnhancedGetPacksResUnk0Unk10>(buffer, obj.Unk10);
            }
        
            public override CDataS2CEquipEnhancedGetPacksResUnk0 Read(IBuffer buffer)
            {
                CDataS2CEquipEnhancedGetPacksResUnk0 obj = new CDataS2CEquipEnhancedGetPacksResUnk0();
                obj.Unk0 = ReadUInt16(buffer);
                obj.Unk1 = ReadByte(buffer);
                obj.Unk2 = ReadMtString(buffer);
                obj.Unk3 = ReadUInt16(buffer);
                obj.Unk4 = ReadUInt16(buffer);
                obj.Unk5 = ReadEntityList<CDataWalletPoint>(buffer);
                obj.Unk6 = ReadEntityList<CDataS2CEquipEnhancedGetPacksResUnk0Unk6>(buffer);
                obj.Unk7 = ReadEntityList<CDataWalletPoint>(buffer);
                obj.Unk8 = ReadEntityList<CDataS2CEquipEnhancedGetPacksResUnk0Unk6>(buffer);
                obj.Unk9 = ReadEntityList<CDataS2CEquipEnhancedGetPacksResUnk0Unk9>(buffer);
                obj.Unk10 = ReadEntityList<CDataS2CEquipEnhancedGetPacksResUnk0Unk10>(buffer);
                return obj;
            }
        }
    }
}