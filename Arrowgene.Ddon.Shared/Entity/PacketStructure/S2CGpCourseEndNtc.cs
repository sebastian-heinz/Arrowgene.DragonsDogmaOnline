using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGpCourseEndNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_GP_COURSE_END_NTC;

        public UInt32 CourseID { get; set; }
        public uint AnnounceType { get; set; }

        public class Serializer : PacketEntitySerializer<S2CGpCourseEndNtc>
        {
            public override void Write(IBuffer buffer, S2CGpCourseEndNtc obj)
            {
                WriteUInt32(buffer, obj.CourseID);
                WriteUInt32(buffer, obj.AnnounceType);
            }

            public override S2CGpCourseEndNtc Read(IBuffer buffer)
            {
                S2CGpCourseEndNtc obj = new S2CGpCourseEndNtc();
                obj.CourseID = ReadUInt32(buffer);
                obj.AnnounceType = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
