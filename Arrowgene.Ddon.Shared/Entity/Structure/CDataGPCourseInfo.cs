using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGPCourseInfo
    {
        public uint CourseId { get; set; }
        public string CourseName { get; set; } = "unknown";
        public bool DoubleCourseTarget { get; set; }
        public byte PrioGroup { get; set; }
        public byte PrioSameTime { get; set; }
        public byte AnnounceType { get; set; }
        public List<CDataCommonU32> EffectUIDs { get; set; } = [];

        public class Serializer : EntitySerializer<CDataGPCourseInfo>
        {
            public override void Write(IBuffer buffer, CDataGPCourseInfo obj)
            {
                WriteUInt32(buffer, obj.CourseId);
                WriteMtString(buffer, obj.CourseName);
                WriteBool(buffer, obj.DoubleCourseTarget);
                WriteByte(buffer, obj.PrioGroup);
                WriteByte(buffer, obj.PrioSameTime);
                WriteByte(buffer, obj.AnnounceType);
                WriteEntityList(buffer, obj.EffectUIDs);
            }

            public override CDataGPCourseInfo Read(IBuffer buffer)
            {
                CDataGPCourseInfo obj = new CDataGPCourseInfo();
                obj.CourseId = ReadUInt32(buffer);
                obj.CourseName = ReadMtString(buffer);
                obj.DoubleCourseTarget = ReadBool(buffer);
                obj.PrioGroup = ReadByte(buffer);
                obj.PrioSameTime = ReadByte(buffer);
                obj.AnnounceType = ReadByte(buffer);
                obj.EffectUIDs = ReadEntityList<CDataCommonU32>(buffer);

                return obj;
            }
        }
    }
}
