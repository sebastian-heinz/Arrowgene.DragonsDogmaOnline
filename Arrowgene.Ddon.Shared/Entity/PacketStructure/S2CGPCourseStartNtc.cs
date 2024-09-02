using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGPCourseStartNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_GP_COURSE_START_NTC;

        public uint CourseID { get; set; }
        public ulong ExpiryTimestamp { get; set; }
        public uint AnnounceType {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CGPCourseStartNtc>
        {
            public override void Write(IBuffer buffer, S2CGPCourseStartNtc obj)
            {
                WriteUInt32(buffer, obj.CourseID);
                WriteUInt64(buffer, obj.ExpiryTimestamp);
                WriteUInt32(buffer, obj.AnnounceType);
            }

            public override S2CGPCourseStartNtc Read(IBuffer buffer)
            {
                S2CGPCourseStartNtc obj = new S2CGPCourseStartNtc();
                obj.CourseID = ReadUInt32(buffer);
                obj.ExpiryTimestamp = ReadUInt64(buffer);
                obj.AnnounceType = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
