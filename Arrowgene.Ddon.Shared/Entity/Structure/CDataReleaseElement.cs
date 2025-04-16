using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataReleaseElement
{
    public JobTrainingReleaseType ReleaseType { get; set; }
    public uint ReleaseId { get; set; }
    public byte ReleaseLv { get; set; }

    public class Serializer : EntitySerializer<CDataReleaseElement>
    {
        public override void Write(IBuffer buffer, CDataReleaseElement obj)
        {
            WriteByte(buffer, (byte) obj.ReleaseType);
            WriteUInt32(buffer, obj.ReleaseId);
            WriteByte(buffer, obj.ReleaseLv);
        }

        public override CDataReleaseElement Read(IBuffer buffer)
        {
            CDataReleaseElement obj = new CDataReleaseElement();
            obj.ReleaseType = (JobTrainingReleaseType) ReadByte(buffer);
            obj.ReleaseId = ReadUInt32(buffer);
            obj.ReleaseLv = ReadByte(buffer);
            return obj;
        }
    }
}
