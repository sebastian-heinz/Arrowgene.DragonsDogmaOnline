using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataQuestTalkInfo
{
    public uint TalkNo { get; set; }
    public ushort NpcId { get; set; }
    public bool IsOneOnly { get; set; }
    
    public class Serializer : EntitySerializer<CDataQuestTalkInfo>
    {
        public override void Write(IBuffer buffer, CDataQuestTalkInfo obj)
        {
            WriteUInt32(buffer, obj.TalkNo);
            WriteUInt16(buffer, obj.NpcId);
            WriteBool(buffer, obj.IsOneOnly);
        }

        public override CDataQuestTalkInfo Read(IBuffer buffer)
        {
            CDataQuestTalkInfo obj = new CDataQuestTalkInfo();
            obj.TalkNo = ReadUInt32(buffer);
            obj.NpcId = ReadUInt16(buffer);
            obj.IsOneOnly = ReadBool(buffer);
            return obj;
        }
    }
}
