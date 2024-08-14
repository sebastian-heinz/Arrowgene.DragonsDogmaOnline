using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGpGpCourseGetAvailableListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_GP_GP_COURSE_GET_AVAILABLE_LIST_RES;

        public List<CDataGPCourseAvailable> Items { get; set; }

        public S2CGpGpCourseGetAvailableListRes()
        {
            Items = new List<CDataGPCourseAvailable>();
        }

        public class Serializer : PacketEntitySerializer<S2CGpGpCourseGetAvailableListRes>
        {
            public override void Write(IBuffer buffer, S2CGpGpCourseGetAvailableListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataGPCourseAvailable>(buffer, obj.Items);
            }

            public override S2CGpGpCourseGetAvailableListRes Read(IBuffer buffer)
            {
                S2CGpGpCourseGetAvailableListRes obj = new S2CGpGpCourseGetAvailableListRes();
                ReadServerResponse(buffer, obj);
                obj.Items = ReadEntityList<CDataGPCourseAvailable>(buffer);
                return obj;
            }
        }
    }
}
