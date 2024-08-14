using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGPCourseValid
    {
        public CDataGPCourseValid()
        {
        }

        public uint ID;
        public uint CourseID;
        public string Name;
        public string ImageAddr;
        public ulong StartTime;
        public ulong EndTime;
        
        public class Serializer : EntitySerializer<CDataGPCourseValid>
        {
            public override void Write(IBuffer buffer, CDataGPCourseValid obj)
            {
                WriteUInt32(buffer, obj.ID);
                WriteUInt32(buffer, obj.CourseID);
                WriteMtString(buffer, obj.Name);
                WriteMtString(buffer, obj.ImageAddr);
                WriteUInt64(buffer, obj.StartTime);
                WriteUInt64(buffer, obj.EndTime);
            }

            public override CDataGPCourseValid Read(IBuffer buffer)
            {
                CDataGPCourseValid obj = new CDataGPCourseValid();
                obj.ID = ReadUInt32(buffer);
                obj.CourseID = ReadUInt32(buffer);
                obj.Name = ReadMtString(buffer);
                obj.ImageAddr = ReadMtString(buffer);
                obj.StartTime = ReadUInt64(buffer);
                obj.EndTime = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}
