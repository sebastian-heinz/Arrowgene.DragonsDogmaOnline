using Arrowgene.Buffers;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGPCourseInfo
    {
        public CDataGPCourseInfo()
        {
            CourseId = 0;
            CourseName = "unknown";
            DoubleCourseTarget = false;
            PrioGroup = 0;
            PrioSameTime = 0;
            AnnounceType = 0;
            EffectUIDs = new List<UInt32>();
        }

        public UInt32 CourseId { get; set; }
        public string CourseName { get; set; }
        public bool DoubleCourseTarget;
        public byte PrioGroup;
        public byte PrioSameTime;
        public byte AnnounceType;
        public List<UInt32> EffectUIDs;

        public class Serializer : EntitySerializer<CDataGPCourseInfo>
        {
            public override void Write(IBuffer buffer, CDataGPCourseInfo obj)
            {
                WriteUInt32(buffer, obj.CourseId);
                WriteMtString(buffer, obj.CourseName);
                WriteByte(buffer, Convert.ToByte(obj.DoubleCourseTarget));
                WriteByte(buffer, obj.PrioGroup);
                WriteByte(buffer, obj.PrioSameTime);
                WriteByte(buffer, obj.AnnounceType);
                WriteMtArray<UInt32>(buffer, obj.EffectUIDs, WriteEffectUID);
            }

            public override CDataGPCourseInfo Read(IBuffer buffer)
            {
                CDataGPCourseInfo obj = new CDataGPCourseInfo();
                obj.CourseId = ReadUInt32(buffer);
                obj.CourseName = ReadMtString(buffer);
                obj.DoubleCourseTarget = Convert.ToBoolean(ReadByte(buffer));
                obj.PrioGroup = ReadByte(buffer);
                obj.PrioSameTime = ReadByte(buffer);
                obj.AnnounceType = ReadByte(buffer);
                obj.EffectUIDs = ReadMtArray<UInt32>(buffer, ReadEffectUID);

                return obj;
            }
            private UInt32 ReadEffectUID(IBuffer buffer)
            {
                return ReadUInt32(buffer);
            }

            private void WriteEffectUID(IBuffer buffer, UInt32 Value)
            {
                WriteUInt32(buffer, Value);
            }
        }
    }
}
