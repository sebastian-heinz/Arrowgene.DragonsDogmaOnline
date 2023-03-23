using System;
using Arrowgene.Buffers;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGoodsParamUnk7 : ICloneable
    {    
        public uint Unk0 { get; set; }
        public uint Unk1 { get; set; }
        public bool Unk2 { get; set; }
        public uint Unk3 { get; set; }
        public bool Unk4 { get; set; }
        public uint Unk5 { get; set; }
        public uint Unk6 { get; set; }
        public uint Unk7 { get; set; }
        public uint Unk8 { get; set; }
        public uint Unk9 { get; set; }
        public ulong Unk10 { get; set; }
        public ulong Unk11 { get; set; }

        public object Clone()
        {
            return new CDataGoodsParamUnk7()
            {
                Unk0 = this.Unk0,
                Unk1 = this.Unk1,
                Unk2 = this.Unk2,
                Unk3 = this.Unk3,
                Unk4 = this.Unk4,
                Unk5 = this.Unk5,
                Unk6 = this.Unk6,
                Unk7 = this.Unk7,
                Unk8 = this.Unk8,
                Unk9 = this.Unk9,
                Unk10 = this.Unk10,
                Unk11 = this.Unk11
            };
        }

        public class Serializer : EntitySerializer<CDataGoodsParamUnk7>
        {
            public override void Write(IBuffer buffer, CDataGoodsParamUnk7 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteBool(buffer, obj.Unk2);
                WriteUInt32(buffer, obj.Unk3);
                WriteBool(buffer, obj.Unk4);
                WriteUInt32(buffer, obj.Unk5);
                WriteUInt32(buffer, obj.Unk6);
                WriteUInt32(buffer, obj.Unk7);
                WriteUInt32(buffer, obj.Unk8);
                WriteUInt32(buffer, obj.Unk9);
                WriteUInt64(buffer, obj.Unk10);
                WriteUInt64(buffer, obj.Unk11);
            }
        
            public override CDataGoodsParamUnk7 Read(IBuffer buffer)
            {
                CDataGoodsParamUnk7 obj = new CDataGoodsParamUnk7();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2 = ReadBool(buffer);
                obj.Unk3 = ReadUInt32(buffer);
                obj.Unk4 = ReadBool(buffer);
                obj.Unk5 = ReadUInt32(buffer);
                obj.Unk6 = ReadUInt32(buffer);
                obj.Unk7 = ReadUInt32(buffer);
                obj.Unk8 = ReadUInt32(buffer);
                obj.Unk9 = ReadUInt32(buffer);
                obj.Unk10 = ReadUInt64(buffer);
                obj.Unk11 = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}