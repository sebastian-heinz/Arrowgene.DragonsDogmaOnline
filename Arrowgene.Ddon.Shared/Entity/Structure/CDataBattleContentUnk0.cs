using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBattleContentUnk0
    {
        public CDataBattleContentUnk0()
        {
        }

        public uint Unk0 { get; set; } // 0x4
        public ulong Unk1 { get; set; } // 0x8
        public bool Unk2 { get; set; } // 0x10
        public bool Unk3 { get; set; } // 0x11
        public byte Unk4 { get; set; } // 0x12
        public uint Unk5 { get; set; } // 0x14
        public uint Unk6 { get; set; } // 0x18
        public uint Unk7 { get; set; } // 0x1c
        public byte Unk8 { get; set; } // 0x20
        public ulong Unk9 { get; set; } // 0x28
        public uint Unk10 { get; set; } // 0x30
        public uint Unk11 { get; set; } // 0x34

        public class Serializer : EntitySerializer<CDataBattleContentUnk0>
        {
            public override void Write(IBuffer buffer, CDataBattleContentUnk0 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt64(buffer, obj.Unk1);
                WriteBool(buffer, obj.Unk2);
                WriteBool(buffer, obj.Unk3);
                WriteByte(buffer, obj.Unk4);
                WriteUInt32(buffer, obj.Unk5);
                WriteUInt32(buffer, obj.Unk6);
                WriteUInt32(buffer, obj.Unk7);
                WriteByte(buffer, obj.Unk8);
                WriteUInt64(buffer, obj.Unk9);
                WriteUInt32(buffer, obj.Unk10);
                WriteUInt32(buffer, obj.Unk11);
            }

            public override CDataBattleContentUnk0 Read(IBuffer buffer)
            {
                CDataBattleContentUnk0 obj = new CDataBattleContentUnk0();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt64(buffer);
                obj.Unk2 = ReadBool(buffer);
                obj.Unk3 = ReadBool(buffer);
                obj.Unk4 = ReadByte(buffer);
                obj.Unk5 = ReadUInt32(buffer);
                obj.Unk6 = ReadUInt32(buffer);
                obj.Unk7 = ReadUInt32(buffer);
                obj.Unk8 = ReadByte(buffer);
                obj.Unk9 = ReadUInt64(buffer);
                obj.Unk10 = ReadUInt32(buffer);
                obj.Unk11 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

