using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataResetInfoUnk0
{
    public CDataResetInfoUnk0()
    {

    }

    /// <summary>
    /// Passed to C2SBattleContentContentResetReq.
    /// </summary>
    public uint Index { get; set; }
    public bool IsPremium1 { get; set; } // Needs to be True for the GG reset entry.
    public bool IsPremium2 { get; set; } // Needs to be True for the GG reset entry.
    public bool Unk3 { get; set; }
    public byte Unk4 { get; set; }
    public bool Unk5 { get; set; }

    public class Serializer : EntitySerializer<CDataResetInfoUnk0>
    {
        public override void Write(IBuffer buffer, CDataResetInfoUnk0 obj)
        {
            WriteUInt32(buffer, obj.Index);
            WriteBool(buffer, obj.IsPremium1);
            WriteBool(buffer, obj.IsPremium2);
            WriteBool(buffer, obj.Unk3);
            WriteByte(buffer, obj.Unk4);
            WriteBool(buffer, obj.Unk5);
        }

        public override CDataResetInfoUnk0 Read(IBuffer buffer)
        {
            CDataResetInfoUnk0 obj = new CDataResetInfoUnk0();
            obj.Index = ReadUInt32(buffer);
            obj.IsPremium1 = ReadBool(buffer);
            obj.IsPremium2 = ReadBool(buffer);
            obj.Unk3 = ReadBool(buffer);
            obj.Unk4 = ReadByte(buffer);
            obj.Unk5 = ReadBool(buffer);
            return obj;
        }
    }
}
