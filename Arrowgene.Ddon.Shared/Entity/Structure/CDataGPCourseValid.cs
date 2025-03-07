using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGPCourseValid
    {
        public CDataGPCourseValid()
        {
        }

        public uint Id { get; set; }
        public uint CourseId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageAddr { get; set; } = string.Empty;
        public ulong StartTime { get; set; }
        public ulong EndTime { get; set; }

        public class Serializer : EntitySerializer<CDataGPCourseValid>
        {
            public override void Write(IBuffer buffer, CDataGPCourseValid obj)
            {
                WriteUInt32(buffer, obj.Id);
                WriteUInt32(buffer, obj.CourseId);
                WriteMtString(buffer, obj.Name);
                WriteMtString(buffer, obj.ImageAddr);
                WriteUInt64(buffer, obj.StartTime);
                WriteUInt64(buffer, obj.EndTime);
            }

            public override CDataGPCourseValid Read(IBuffer buffer)
            {
                CDataGPCourseValid obj = new CDataGPCourseValid();
                obj.Id = ReadUInt32(buffer);
                obj.CourseId = ReadUInt32(buffer);
                obj.Name = ReadMtString(buffer);
                obj.ImageAddr = ReadMtString(buffer);
                obj.StartTime = ReadUInt64(buffer);
                obj.EndTime = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}
