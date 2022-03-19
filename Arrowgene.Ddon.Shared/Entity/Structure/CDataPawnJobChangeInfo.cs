using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPawnJobChangeInfo
    {
        public CDataPawnJobChangeInfo()
        {
            SlotNo=0;
            PawnId=0;
            JobChangeInfoList=new List<CDataJobChangeInfo>();
            JobReleaseInfoList=new List<CDataJobChangeInfo>();
        }

        public byte SlotNo { get; set; }
        public uint PawnId { get; set; }
        public List<CDataJobChangeInfo> JobChangeInfoList { get; set; }
        public List<CDataJobChangeInfo> JobReleaseInfoList { get; set; }

        public class Serializer : EntitySerializer<CDataPawnJobChangeInfo>
        {
            public override void Write(IBuffer buffer, CDataPawnJobChangeInfo obj)
            {
                WriteByte(buffer, obj.SlotNo);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList<CDataJobChangeInfo>(buffer, obj.JobChangeInfoList);
                WriteEntityList<CDataJobChangeInfo>(buffer, obj.JobReleaseInfoList);
            }

            public override CDataPawnJobChangeInfo Read(IBuffer buffer)
            {
                CDataPawnJobChangeInfo obj = new CDataPawnJobChangeInfo();
                obj.SlotNo = ReadByte(buffer);
                obj.PawnId = ReadUInt32(buffer);
                obj.JobChangeInfoList = ReadEntityList<CDataJobChangeInfo>(buffer);
                obj.JobReleaseInfoList = ReadEntityList<CDataJobChangeInfo>(buffer);
                return obj;
            }
        }
    }
}
