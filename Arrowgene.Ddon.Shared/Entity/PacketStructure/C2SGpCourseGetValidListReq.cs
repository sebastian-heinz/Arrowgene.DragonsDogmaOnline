using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure;

public class C2SGpCourseGetValidListReq : IPacketStructure
{
    public PacketId Id => PacketId.C2S_GP_GP_COURSE_GET_VALID_LIST_REQ;

    public class Serializer : PacketEntitySerializer<C2SGpCourseGetValidListReq>
    {
        public override void Write(IBuffer buffer, C2SGpCourseGetValidListReq obj)
        {
        }

        public override C2SGpCourseGetValidListReq Read(IBuffer buffer)
        {
            var obj = new C2SGpCourseGetValidListReq();

            return obj;
        }
    }
}
