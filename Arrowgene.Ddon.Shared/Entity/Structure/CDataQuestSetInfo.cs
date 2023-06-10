using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataQuestSetInfo
    {
        public uint StageNo { get; set; }
        public uint GroupId { get; set; }
    
        public class Serializer : EntitySerializer<CDataQuestSetInfo>
        {
            public override void Write(IBuffer buffer, CDataQuestSetInfo obj)
            {
                WriteUInt32(buffer, obj.StageNo);
                WriteUInt32(buffer, obj.GroupId);
            }
        
            public override CDataQuestSetInfo Read(IBuffer buffer)
            {
                CDataQuestSetInfo obj = new CDataQuestSetInfo();
                obj.StageNo = ReadUInt32(buffer);
                obj.GroupId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}