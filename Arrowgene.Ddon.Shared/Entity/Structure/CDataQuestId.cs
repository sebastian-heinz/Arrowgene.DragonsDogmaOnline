using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataQuestId
    {
        public uint QuestId { get; set; }
    
        public class Serializer : EntitySerializer<CDataQuestId>
        {
            public override void Write(IBuffer buffer, CDataQuestId obj)
            {
                WriteUInt32(buffer, obj.QuestId);
            }
        
            public override CDataQuestId Read(IBuffer buffer)
            {
                CDataQuestId obj = new CDataQuestId();
                obj.QuestId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}