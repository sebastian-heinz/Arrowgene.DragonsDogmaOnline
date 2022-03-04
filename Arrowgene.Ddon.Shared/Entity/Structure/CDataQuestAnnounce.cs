using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataQuestAnnounce
{
    public CDataQuestAnnounce()
    {

    }
    
    public class Serializer : EntitySerializer<CDataQuestAnnounce>
    {
        public override void Write(IBuffer buffer, CDataQuestAnnounce obj)
        {

        }

        public override CDataQuestAnnounce Read(IBuffer buffer)
        {
            CDataQuestAnnounce obj = new CDataQuestAnnounce();
            return obj;
        }
    }
}
