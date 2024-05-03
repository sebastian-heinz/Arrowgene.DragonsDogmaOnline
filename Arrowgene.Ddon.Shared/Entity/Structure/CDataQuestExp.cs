using Arrowgene.Buffers;
using System;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataQuestExp
{
    public byte Unk0 { get; set; }
    public byte ExpMode { get; set; }
    public UInt32 Reward { get; set; }

    public class Serializer : EntitySerializer<CDataQuestExp>
    {
        public override void Write(IBuffer buffer, CDataQuestExp obj)
        {
            WriteByte(buffer, obj.Unk0);
            WriteByte(buffer, obj.ExpMode);
            WriteUInt32(buffer, obj.Reward);
        }

        public override CDataQuestExp Read(IBuffer buffer)
        {
            CDataQuestExp obj = new CDataQuestExp();
            obj.Unk0 = ReadByte(buffer);
            obj.ExpMode = ReadByte(buffer);
            obj.Reward = ReadUInt32(buffer);
            return obj;
        }
    }
}

