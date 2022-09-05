using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataContextAcquirementData
    {
        public CDataContextAcquirementData(CDataSetAcquirementParam setAcquirementParam)
        {
            SlotNo = setAcquirementParam.SlotNo;
            AcquirementNo = setAcquirementParam.AcquirementNo;
            AcquirementLv = setAcquirementParam.AcquirementLv;
        }

        public CDataContextAcquirementData()
        {
            SlotNo = 0;
            AcquirementNo = 0;
            AcquirementLv = 0;
        }

        public byte SlotNo { get; set; }
        public uint AcquirementNo { get; set; }
        public byte AcquirementLv { get; set; }

        public class Serializer : EntitySerializer<CDataContextAcquirementData>
        {
            public override void Write(IBuffer buffer, CDataContextAcquirementData obj)
            {
                WriteByte(buffer, obj.SlotNo);
                WriteUInt32(buffer, obj.AcquirementNo);
                WriteByte(buffer, obj.AcquirementLv);
            }

            public override CDataContextAcquirementData Read(IBuffer buffer)
            {
                CDataContextAcquirementData obj = new CDataContextAcquirementData();
                obj.SlotNo = ReadByte(buffer);
                obj.AcquirementNo = ReadUInt32(buffer);
                obj.AcquirementLv = ReadByte(buffer);
                return obj;
            }
        }
    }
}