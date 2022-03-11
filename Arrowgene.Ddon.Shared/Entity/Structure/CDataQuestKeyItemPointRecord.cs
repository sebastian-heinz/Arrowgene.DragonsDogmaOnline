using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataQuestKeyItemPointRecord
{
    public CDataQuestKeyItemPointRecord()
    {

    }
    
    public class Serializer : EntitySerializer<CDataQuestKeyItemPointRecord>
    {
        public override void Write(IBuffer buffer, CDataQuestKeyItemPointRecord obj)
        {

        }

        public override CDataQuestKeyItemPointRecord Read(IBuffer buffer)
        {
            CDataQuestKeyItemPointRecord obj = new CDataQuestKeyItemPointRecord();
            return obj;
        }
    }
}
