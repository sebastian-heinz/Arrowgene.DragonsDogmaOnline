using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataQuestAnnounce
{
    public uint AnnounceNo { get; set; }
    
    public class Serializer : EntitySerializer<CDataQuestAnnounce>
    {
        public override void Write(IBuffer buffer, CDataQuestAnnounce obj)
        {
            WriteUInt32(buffer, obj.AnnounceNo);
        }

        public override CDataQuestAnnounce Read(IBuffer buffer)
        {
            CDataQuestAnnounce obj = new CDataQuestAnnounce();
            obj.AnnounceNo = ReadUInt32(buffer);
            return obj;
        }
    }
}
