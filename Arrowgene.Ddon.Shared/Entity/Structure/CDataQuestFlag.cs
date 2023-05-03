using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataQuestFlag
{
    public uint FlagId { get; set; }
    
    public class Serializer : EntitySerializer<CDataQuestFlag>
    {
        public override void Write(IBuffer buffer, CDataQuestFlag obj)
        {
            WriteUInt32(buffer, obj.FlagId);
        }

        public override CDataQuestFlag Read(IBuffer buffer)
        {
            CDataQuestFlag obj = new CDataQuestFlag();
            obj.FlagId = ReadUInt32(buffer);
            return obj;
        }
    }
}
