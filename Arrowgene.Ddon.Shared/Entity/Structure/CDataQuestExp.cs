using System;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataQuestExp
{
    public byte Unk0 { get; set; }
    public PointType Type { get; set; }
    public UInt32 Reward { get; set; }

    public class Serializer : EntitySerializer<CDataQuestExp>
    {
        public override void Write(IBuffer buffer, CDataQuestExp obj)
        {
            WriteByte(buffer, obj.Unk0);
            WriteByte(buffer, (byte)obj.Type);
            WriteUInt32(buffer, obj.Reward);
        }

        public override CDataQuestExp Read(IBuffer buffer)
        {
            CDataQuestExp obj = new CDataQuestExp();
            obj.Unk0 = ReadByte(buffer);
            obj.Type = (PointType)ReadByte(buffer);
            obj.Reward = ReadUInt32(buffer);
            return obj;
        }
    }
}
