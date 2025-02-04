using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGPCourseValid
    {
        public CDataGPCourseValid()
        {
            Id = 0;
            CourseId = 0;
            NameA = "";
            NameB = "";
            StartTime = 0;
            EndTime = 0;
        }

        public uint Id;
        public uint CourseId;
        public string NameA;
        public string NameB;
        public ulong StartTime;
        public ulong EndTime;

        public class Serializer : EntitySerializer<CDataGPCourseValid>
        {
            public override void Write(IBuffer buffer, CDataGPCourseValid obj)
            {
                WriteUInt32(buffer, obj.Id);
                WriteUInt32(buffer, obj.CourseId);
                WriteMtString(buffer, obj.NameA);
                WriteMtString(buffer, obj.NameB);
                WriteUInt64(buffer, obj.StartTime); // TODO verify endianness big
                WriteUInt64(buffer, obj.EndTime);
            }

            public override CDataGPCourseValid Read(IBuffer buffer)
            {
                CDataGPCourseValid obj = new CDataGPCourseValid();
                obj.Id = ReadUInt32(buffer);
                obj.CourseId = ReadUInt32(buffer);
                obj.NameA = ReadMtString(buffer);
                obj.NameB = ReadMtString(buffer);
                obj.StartTime = ReadUInt64(buffer);
                obj.EndTime = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}
