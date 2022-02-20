using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobGetJobChangeListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_JOB_GET_JOB_CHANGE_LIST_RES;

        public S2CJobGetJobChangeListRes()
        {
            JobChangeInfo=new List<CDataJobChangeInfo>();
            JobReleaseInfo=new List<CDataJobChangeInfo>();
            PawnJobChangeInfoList=new List<CDataPawnJobChangeInfo>();
            PlayPointList=new List<CDataJobPlayPoint>();
        }

        public List<CDataJobChangeInfo> JobChangeInfo { get; set; }
        public List<CDataJobChangeInfo> JobReleaseInfo { get; set; }
        public List<CDataPawnJobChangeInfo> PawnJobChangeInfoList { get; set; }
        public List<CDataJobPlayPoint> PlayPointList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CJobGetJobChangeListRes>
        {
            public override void Write(IBuffer buffer, S2CJobGetJobChangeListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataJobChangeInfo>(buffer, obj.JobChangeInfo);
                WriteEntityList<CDataJobChangeInfo>(buffer, obj.JobReleaseInfo);
                WriteEntityList<CDataPawnJobChangeInfo>(buffer, obj.PawnJobChangeInfoList);
                WriteEntityList<CDataJobPlayPoint>(buffer, obj.PlayPointList);
            }

            public override S2CJobGetJobChangeListRes Read(IBuffer buffer)
            {
                S2CJobGetJobChangeListRes obj = new S2CJobGetJobChangeListRes();
                ReadServerResponse(buffer, obj);
                obj.JobChangeInfo = ReadEntityList<CDataJobChangeInfo>(buffer);
                obj.JobReleaseInfo = ReadEntityList<CDataJobChangeInfo>(buffer);
                obj.PawnJobChangeInfoList = ReadEntityList<CDataPawnJobChangeInfo>(buffer);
                obj.PlayPointList = ReadEntityList<CDataJobPlayPoint>(buffer);
                return obj;
            }
        }
    }
}
