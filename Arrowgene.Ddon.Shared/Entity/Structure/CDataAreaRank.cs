using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataAreaRank
    {
        public QuestAreaId AreaId { get; set; }
        public uint Rank { get; set; }
    
        public class Serializer : EntitySerializer<CDataAreaRank>
        {
            public override void Write(IBuffer buffer, CDataAreaRank obj)
            {
                WriteUInt32(buffer, (uint)obj.AreaId);
                WriteUInt32(buffer, obj.Rank);
            }
        
            public override CDataAreaRank Read(IBuffer buffer)
            {
                CDataAreaRank obj = new CDataAreaRank();
                obj.AreaId = (QuestAreaId)ReadUInt32(buffer);
                obj.Rank = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
