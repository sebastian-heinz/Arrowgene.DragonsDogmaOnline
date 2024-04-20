using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGpGpCourseGetAvailableListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_GP_GP_COURSE_GET_AVAILABLE_LIST_RES;

        public S2CGpGpCourseGetAvailableListRes()
        {
            AvailableCourses = new List<CDataGPCourseInfo>();
        }

        public List<CDataGPCourseInfo> AvailableCourses { get; set; }

        public class Serializer : PacketEntitySerializer<S2CGpGpCourseGetAvailableListRes>
        {
            public override void Write(IBuffer buffer, S2CGpGpCourseGetAvailableListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataGPCourseInfo>(buffer, obj.AvailableCourses);
            }

            public override S2CGpGpCourseGetAvailableListRes Read(IBuffer buffer)
            {
                S2CGpGpCourseGetAvailableListRes obj = new S2CGpGpCourseGetAvailableListRes();
                ReadServerResponse(buffer, obj);
                obj.AvailableCourses = ReadEntityList<CDataGPCourseInfo>(buffer);
                return obj;
            }
        }
    }
}
