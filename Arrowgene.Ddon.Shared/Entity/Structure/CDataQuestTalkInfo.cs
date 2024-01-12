using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataQuestTalkInfo
{
    // One of these two is TalkNo
    public uint Unk0 { get; set; }
    public uint Unk1 { get; set; }
    public ushort NpcId { get; set; }
    public bool IsOneOnly { get; set; }
    
    public class Serializer : EntitySerializer<CDataQuestTalkInfo>
    {
        public override void Write(IBuffer buffer, CDataQuestTalkInfo obj)
        {
            WriteUInt32(buffer, obj.Unk0);
            WriteUInt32(buffer, obj.Unk1);
            WriteUInt16(buffer, obj.NpcId);
            WriteBool(buffer, obj.IsOneOnly);
        }

        public override CDataQuestTalkInfo Read(IBuffer buffer)
        {
            CDataQuestTalkInfo obj = new CDataQuestTalkInfo();
            obj.Unk0 = ReadUInt32(buffer);
            obj.Unk1 = ReadUInt32(buffer);
            obj.NpcId = ReadUInt16(buffer);
            obj.IsOneOnly = ReadBool(buffer);
            return obj;
        }
    }
}
