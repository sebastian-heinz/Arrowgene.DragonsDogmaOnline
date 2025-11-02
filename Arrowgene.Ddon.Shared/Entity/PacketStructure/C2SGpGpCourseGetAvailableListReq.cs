using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SGpGpCourseGetAvailableListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_GP_GP_COURSE_GET_AVAILABLE_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SGpGpCourseGetAvailableListReq>
        {
            public override void Write(IBuffer buffer, C2SGpGpCourseGetAvailableListReq obj)
            {
            }

            public override C2SGpGpCourseGetAvailableListReq Read(IBuffer buffer)
            {
                C2SGpGpCourseGetAvailableListReq obj = new C2SGpGpCourseGetAvailableListReq();
                return obj;
            }
        }
    }
}
