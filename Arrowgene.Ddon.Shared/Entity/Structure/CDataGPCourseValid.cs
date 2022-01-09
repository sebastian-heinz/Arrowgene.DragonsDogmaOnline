using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGPCourseValid
    {
        public CDataGPCourseValid()
        {
            ID = 0;
            CourseID = 0;
            NameA = "";
            NameB = "";
            StartTime = 0;
            EndTime = 0;
        }

        public uint ID;
        public uint CourseID;
        public string NameA;
        public string NameB;
        public ulong StartTime;
        public ulong EndTime;
    }

    public class CDataGPCourseValidSerializer : EntitySerializer<CDataGPCourseValid>
    {
        public override void Write(IBuffer buffer, CDataGPCourseValid obj)
        {
            WriteUInt32(buffer, obj.ID);
            WriteUInt32(buffer, obj.CourseID);
            WriteMtString(buffer, obj.NameA);
            WriteMtString(buffer, obj.NameB);
            WriteUInt64(buffer, obj.StartTime); // TODO verify endianness big
            WriteUInt64(buffer, obj.EndTime);
        }

        public override CDataGPCourseValid Read(IBuffer buffer)
        {
            CDataGPCourseValid obj = new CDataGPCourseValid();
            obj.ID = ReadUInt32(buffer);
            obj.CourseID = ReadUInt32(buffer);
            obj.NameA = ReadMtString(buffer);
            obj.NameB = ReadMtString(buffer);
            obj.StartTime = ReadUInt64(buffer);
            obj.EndTime = ReadUInt64(buffer);
            return obj;
        }
    }
}
