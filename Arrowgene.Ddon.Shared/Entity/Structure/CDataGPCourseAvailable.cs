using Arrowgene.Buffers;
using System;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGPCourseAvailable
    {
        public CDataGPCourseAvailable()
        {
            CourseID = 0;
            CourseName = "";
        }

        // Character ID?
        public UInt32 ID { get; set; }
        public string CourseName { get; set; }
        public UInt64 UseLimitTime { get; set; }
        public UInt32 CourseID { get; set; }
        public UInt32 LineupID { get; set; }
        public UInt32 BackIconID { get; set; }
        public UInt32 FrameIconID { get; set; }

        public class Serializer : EntitySerializer<CDataGPCourseAvailable>
        {
            public override void Write(IBuffer buffer, CDataGPCourseAvailable obj)
            {
                WriteUInt32(buffer, obj.ID);
                WriteMtString(buffer, obj.CourseName);
                WriteUInt64(buffer, obj.UseLimitTime);
                WriteUInt32(buffer, obj.CourseID);
                WriteUInt32(buffer, obj.LineupID);
                WriteUInt32(buffer, obj.BackIconID);
                WriteUInt32(buffer, obj.FrameIconID);
            }

            public override CDataGPCourseAvailable Read(IBuffer buffer)
            {
                CDataGPCourseAvailable obj = new CDataGPCourseAvailable();
                obj.ID = ReadUInt32(buffer);
                obj.CourseName = ReadMtString(buffer);
                obj.UseLimitTime = ReadUInt64(buffer);
                obj.CourseID = ReadUInt32(buffer);
                obj.LineupID = ReadUInt32(buffer);
                obj.BackIconID = ReadUInt32(buffer);
                obj.FrameIconID = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
