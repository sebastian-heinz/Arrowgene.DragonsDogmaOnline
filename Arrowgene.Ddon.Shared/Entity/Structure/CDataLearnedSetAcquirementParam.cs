using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataLearnedSetAcquirementParam
    {
        public CDataLearnedSetAcquirementParam()
        {
            Job=0;
            Type=0;
            AcquirementNo=0;
            AcquirementLv=0;
            AcquirementParamID=0;
        }

        public byte Job { get; set; }
        public byte Type { get; set; }
        public uint AcquirementNo { get; set; }
        public byte AcquirementLv { get; set; }
        public uint AcquirementParamID { get; set; }

        public class Serializer : EntitySerializer<CDataLearnedSetAcquirementParam>
        {
            public override void Write(IBuffer buffer, CDataLearnedSetAcquirementParam obj)
            {
                WriteByte(buffer, obj.Job);
                WriteByte(buffer, obj.Type);
                WriteUInt32(buffer, obj.AcquirementNo);
                WriteByte(buffer, obj.AcquirementLv);
                WriteUInt32(buffer, obj.AcquirementParamID);
            }

            public override CDataLearnedSetAcquirementParam Read(IBuffer buffer)
            {
                CDataLearnedSetAcquirementParam obj = new CDataLearnedSetAcquirementParam();
                obj.Job = ReadByte(buffer);
                obj.Type = ReadByte(buffer);
                obj.AcquirementNo = ReadUInt32(buffer);
                obj.AcquirementLv = ReadByte(buffer);
                obj.AcquirementParamID = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
