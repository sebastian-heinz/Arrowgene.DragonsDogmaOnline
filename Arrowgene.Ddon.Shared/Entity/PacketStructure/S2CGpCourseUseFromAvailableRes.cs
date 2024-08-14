using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure;

public class S2CGpCourseUseFromAvailableRes : ServerResponse
{
    public override PacketId Id => PacketId.S2C_GP_GP_COURSE_USE_FROM_AVAILABLE_RES;

    public ulong FinishDateTime { get; set; }

    public class Serializer : PacketEntitySerializer<S2CGpCourseUseFromAvailableRes>
    {
        public override void Write(IBuffer buffer, S2CGpCourseUseFromAvailableRes obj)
        {
            WriteServerResponse(buffer, obj);

            WriteUInt64(buffer, obj.FinishDateTime);
        }

        public override S2CGpCourseUseFromAvailableRes Read(IBuffer buffer)
        {
            var obj = new S2CGpCourseUseFromAvailableRes();

            ReadServerResponse(buffer, obj);

            obj.FinishDateTime = ReadUInt64(buffer);

            return obj;
        }
    }
}
