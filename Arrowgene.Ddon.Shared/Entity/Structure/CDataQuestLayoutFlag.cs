using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataQuestLayoutFlag
{
    public uint FlagId { get; set; }
    
    public class Serializer : EntitySerializer<CDataQuestLayoutFlag>
    {
        public override void Write(IBuffer buffer, CDataQuestLayoutFlag obj)
        {
            WriteUInt32(buffer, obj.FlagId);
        }

        public override CDataQuestLayoutFlag Read(IBuffer buffer)
        {
            CDataQuestLayoutFlag obj = new CDataQuestLayoutFlag();
            obj.FlagId = ReadUInt32(buffer);
            return obj;
        }
    }
}
