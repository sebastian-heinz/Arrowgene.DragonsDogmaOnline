using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataReleaseAcquirementParam
{
    public CDataReleaseAcquirementParam()
    {
    }

    public JobId JobId { get; set; }
    public byte Type { get; set; }
    public uint AcquirementNo { get; set; }
    public byte AcquirementLv { get; set; }
    public uint AcquirementParamID { get; set; }


    public class Serializer : EntitySerializer<CDataReleaseAcquirementParam>
    {
        public override void Write(IBuffer buffer, CDataReleaseAcquirementParam obj)
        {
            WriteByte(buffer, (byte) obj.JobId);
            WriteByte(buffer, obj.Type);
            WriteUInt32(buffer, obj.AcquirementNo);
            WriteByte(buffer, obj.AcquirementLv);
            WriteUInt32(buffer, obj.AcquirementParamID);
        }

        public override CDataReleaseAcquirementParam Read(IBuffer buffer)
        {
            CDataReleaseAcquirementParam obj = new CDataReleaseAcquirementParam();
            obj.JobId = (JobId)ReadByte(buffer);
            obj.Type = ReadByte(buffer);
            obj.AcquirementNo = ReadUInt32(buffer);
            obj.AcquirementLv = ReadByte(buffer);
            obj.AcquirementParamID = ReadUInt32(buffer);
            return obj;
        }
    }
}
