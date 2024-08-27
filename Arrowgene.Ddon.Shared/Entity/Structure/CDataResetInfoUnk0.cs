using Arrowgene.Buffers;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataResetInfoUnk0
{
    public CDataResetInfoUnk0()
    {

    }

    public uint Unk0 { get; set; }
    public bool Unk1 { get; set; }
    public bool Unk2 { get; set; }
    public bool Unk3 { get; set; }
    public byte Unk4 { get; set; }
    public bool Unk5 { get; set; }

    public class Serializer : EntitySerializer<CDataResetInfoUnk0>
    {
        public override void Write(IBuffer buffer, CDataResetInfoUnk0 obj)
        {
            WriteUInt32(buffer, obj.Unk0);
            WriteBool(buffer, obj.Unk1);
            WriteBool(buffer, obj.Unk2);
            WriteBool(buffer, obj.Unk3);
            WriteByte(buffer, obj.Unk4);
            WriteBool(buffer, obj.Unk5);
        }

        public override CDataResetInfoUnk0 Read(IBuffer buffer)
        {
            CDataResetInfoUnk0 obj = new CDataResetInfoUnk0();
            obj.Unk0 = ReadUInt32(buffer);
            obj.Unk1 = ReadBool(buffer);
            obj.Unk2 = ReadBool(buffer);
            obj.Unk3 = ReadBool(buffer);
            obj.Unk4 = ReadByte(buffer);
            obj.Unk5 = ReadBool(buffer);
            return obj;
        }
    }
}
