using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SGpCourseUseFromAvailableReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_GP_GP_COURSE_USE_FROM_AVAILABLE_REQ;

        public uint AvailableId { get; set; }

        public C2SGpCourseUseFromAvailableReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SGpCourseUseFromAvailableReq>
        {
            public override void Write(IBuffer buffer, C2SGpCourseUseFromAvailableReq obj)
            {
                WriteUInt32(buffer, obj.AvailableId);
            }

            public override C2SGpCourseUseFromAvailableReq Read(IBuffer buffer)
            {
                C2SGpCourseUseFromAvailableReq obj = new C2SGpCourseUseFromAvailableReq();

                obj.AvailableId = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
