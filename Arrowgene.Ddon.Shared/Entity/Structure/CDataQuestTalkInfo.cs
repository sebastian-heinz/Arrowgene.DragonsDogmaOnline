using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataQuestTalkInfo
{
    // One of these two is TalkNo
    public uint Unk0 { get; set; }
    public uint TalkNo { get; set; }
    public ushort NpcId { get; set; }
    public bool IsOneOnly { get; set; }
    public CDataQuestTalkInfo()
    {
    }

    public CDataQuestTalkInfo(NpcId npcId, uint talkNo, bool isOnOnly = false)
    {
        NpcId = (ushort) npcId;
        TalkNo = talkNo;
    }

    public class Serializer : EntitySerializer<CDataQuestTalkInfo>
    {
        public override void Write(IBuffer buffer, CDataQuestTalkInfo obj)
        {
            WriteUInt32(buffer, obj.Unk0);
            WriteUInt32(buffer, obj.TalkNo);
            WriteUInt16(buffer, obj.NpcId);
            WriteBool(buffer, obj.IsOneOnly);
        }

        public override CDataQuestTalkInfo Read(IBuffer buffer)
        {
            CDataQuestTalkInfo obj = new CDataQuestTalkInfo();
            obj.Unk0 = ReadUInt32(buffer);
            obj.TalkNo = ReadUInt32(buffer);
            obj.NpcId = ReadUInt16(buffer);
            obj.IsOneOnly = ReadBool(buffer);
            return obj;
        }
    }
}
