using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2LGpCourseGetInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2L_GP_COURSE_GET_INFO_REQ;

        public class Serializer : PacketEntitySerializer<C2LGpCourseGetInfoReq>
        {

            public override void Write(IBuffer buffer, C2LGpCourseGetInfoReq obj)
            {
            }

            public override C2LGpCourseGetInfoReq Read(IBuffer buffer)
            {
                C2LGpCourseGetInfoReq obj = new C2LGpCourseGetInfoReq();
                return obj;
            }
        }
    }
}
