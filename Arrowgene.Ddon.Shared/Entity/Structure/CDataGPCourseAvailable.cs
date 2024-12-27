using System;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGPCourseAvailable
    {
        public CDataGPCourseAvailable()
        {
        }

        public uint ID { get; set; }
        public string Name { get; set; }
        public DateTimeOffset UseLimitTime { get; set; }
        public uint CourseID { get; set; }
        public uint LineupID { get; set; }
        public string ImageAddr { get; set; }
        
        public class Serializer : EntitySerializer<CDataGPCourseAvailable>
        {
            public override void Write(IBuffer buffer, CDataGPCourseAvailable obj)
            {
                WriteUInt32(buffer, obj.ID);
                WriteMtString(buffer, obj.Name);
                WriteUInt64(buffer, (ulong)obj.UseLimitTime.ToUnixTimeSeconds());
                WriteUInt32(buffer, obj.CourseID);
                WriteUInt32(buffer, obj.LineupID);
                WriteMtString(buffer, obj.ImageAddr);
            }

            public override CDataGPCourseAvailable Read(IBuffer buffer)
            {
                CDataGPCourseAvailable obj = new CDataGPCourseAvailable();

                obj.ID = ReadUInt32(buffer);
                obj.Name = ReadMtString(buffer);
                obj.UseLimitTime = DateTimeOffset.FromUnixTimeSeconds((long)ReadUInt64(buffer));
                obj.CourseID = ReadUInt32(buffer);
                obj.LineupID = ReadUInt32(buffer);
                obj.ImageAddr = ReadMtString(buffer);

                return obj;
            }
        }
    }
}
