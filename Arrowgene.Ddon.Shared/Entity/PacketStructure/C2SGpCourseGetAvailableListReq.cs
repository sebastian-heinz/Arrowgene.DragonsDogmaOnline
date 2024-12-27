using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SGpCourseGetAvailableListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_GP_GP_COURSE_GET_AVAILABLE_LIST_REQ;

        public C2SGpCourseGetAvailableListReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SGpCourseGetAvailableListReq>
        {
            public override void Write(IBuffer buffer, C2SGpCourseGetAvailableListReq obj)
            {
            }

            public override C2SGpCourseGetAvailableListReq Read(IBuffer buffer)
            {
                C2SGpCourseGetAvailableListReq obj = new C2SGpCourseGetAvailableListReq();

                return obj;
            }
        }
    }
}
