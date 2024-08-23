using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGPCourseExtendNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_GP_COURSE_EXTEND_NTC;

        public UInt32 CourseID { get; set; }
        public UInt64 ExpiryTimestamp { get; set; }

        public class Serializer : PacketEntitySerializer<S2CGPCourseExtendNtc>
        {
            public override void Write(IBuffer buffer, S2CGPCourseExtendNtc obj)
            {
                WriteUInt32(buffer, obj.CourseID);
                WriteUInt64(buffer, obj.ExpiryTimestamp);
            }

            public override S2CGPCourseExtendNtc Read(IBuffer buffer)
            {
                S2CGPCourseExtendNtc obj = new S2CGPCourseExtendNtc();
                obj.CourseID = ReadUInt32(buffer);
                obj.ExpiryTimestamp = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}
