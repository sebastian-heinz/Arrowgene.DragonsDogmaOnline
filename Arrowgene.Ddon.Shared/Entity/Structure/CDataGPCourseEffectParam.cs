using Arrowgene.Buffers;
using System;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGPCourseEffectParam
    {
        public CDataGPCourseEffectParam()
        {
            EffectUID = 0;
            EffectID = 0;
            Param0 = 0;
            Param1 = 0;
        }

        public UInt32 EffectUID { get; set; }
        public UInt32 EffectID { get; set; }
        public UInt32 Param0 { get; set; }
        public UInt32 Param1 { get; set; }

        public class Serializer : EntitySerializer<CDataGPCourseEffectParam>
        {
            public override void Write(IBuffer buffer, CDataGPCourseEffectParam obj)
            {
                WriteUInt32(buffer, obj.EffectUID);
                WriteUInt32(buffer, obj.EffectID);
                WriteUInt32(buffer, obj.Param0);
                WriteUInt32(buffer, obj.Param1);
            }

            public override CDataGPCourseEffectParam Read(IBuffer buffer)
            {
                CDataGPCourseEffectParam obj = new CDataGPCourseEffectParam();
                obj.EffectUID = ReadUInt32(buffer);
                obj.EffectID = ReadUInt32(buffer);
                obj.Param0 = ReadUInt32(buffer);
                obj.Param1 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
