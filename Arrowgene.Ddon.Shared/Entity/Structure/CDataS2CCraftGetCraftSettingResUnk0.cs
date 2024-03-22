using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataS2CCraftGetCraftSettingResUnk0
    {
        public CDataS2CCraftGetCraftSettingResUnk0() {
            Unk4 = string.Empty;
            Unk6 = new List<CDataS2CCraftGetCraftSettingResUnk0Unk6>();
        }
    
        public uint Unk0 { get; set; }
        public byte Unk1 { get; set; }
        public uint Unk2 { get; set; }
        public uint Unk3 { get; set; }
        public string Unk4 { get; set; }
        public uint Unk5 { get; set; }
        public List<CDataS2CCraftGetCraftSettingResUnk0Unk6> Unk6 { get; set; }
    
        public class Serializer : EntitySerializer<CDataS2CCraftGetCraftSettingResUnk0>
        {
            public override void Write(IBuffer buffer, CDataS2CCraftGetCraftSettingResUnk0 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Unk2);
                WriteUInt32(buffer, obj.Unk3);
                WriteMtString(buffer, obj.Unk4);
                WriteUInt32(buffer, obj.Unk5);
                WriteEntityList<CDataS2CCraftGetCraftSettingResUnk0Unk6>(buffer, obj.Unk6);
            }
        
            public override CDataS2CCraftGetCraftSettingResUnk0 Read(IBuffer buffer)
            {
                CDataS2CCraftGetCraftSettingResUnk0 obj = new CDataS2CCraftGetCraftSettingResUnk0();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadByte(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                obj.Unk3 = ReadUInt32(buffer);
                obj.Unk4 = ReadMtString(buffer);
                obj.Unk5 = ReadUInt32(buffer);
                obj.Unk6 = ReadEntityList<CDataS2CCraftGetCraftSettingResUnk0Unk6>(buffer);
                return obj;
            }
        }
    }
}