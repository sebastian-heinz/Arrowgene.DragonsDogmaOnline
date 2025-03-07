using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGpCourseStartNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_GP_COURSE_START_NTC;

        public uint CourseID { get; set; }
        public ulong ExpiryTimestamp { get; set; }
        public uint AnnounceType {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CGpCourseStartNtc>
        {
            public override void Write(IBuffer buffer, S2CGpCourseStartNtc obj)
            {
                WriteUInt32(buffer, obj.CourseID);
                WriteUInt64(buffer, obj.ExpiryTimestamp);
                WriteUInt32(buffer, obj.AnnounceType);
            }

            public override S2CGpCourseStartNtc Read(IBuffer buffer)
            {
                S2CGpCourseStartNtc obj = new S2CGpCourseStartNtc();
                obj.CourseID = ReadUInt32(buffer);
                obj.ExpiryTimestamp = ReadUInt64(buffer);
                obj.AnnounceType = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
