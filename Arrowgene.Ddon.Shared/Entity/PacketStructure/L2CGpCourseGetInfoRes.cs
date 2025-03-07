using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class L2CGpCourseGetInfoRes : ServerResponse
    {
        public L2CGpCourseGetInfoRes()
        {
            CourseInfo = new List<CDataGPCourseInfo>();
            Effects = new List<CDataGPCourseEffectParam>();
        }

        public List<CDataGPCourseInfo> CourseInfo { get; set; }
        public List<CDataGPCourseEffectParam> Effects { get; set; }
        public uint Version { get; set; }

        public override PacketId Id => PacketId.L2C_GP_COURSE_GET_INFO_RES;

        public class Serializer : PacketEntitySerializer<L2CGpCourseGetInfoRes>
        {

            public override void Write(IBuffer buffer, L2CGpCourseGetInfoRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataGPCourseInfo>(buffer, obj.CourseInfo);
                WriteEntityList<CDataGPCourseEffectParam>(buffer, obj.Effects);
                WriteUInt32(buffer, obj.Version);
            }

            public override L2CGpCourseGetInfoRes Read(IBuffer buffer)
            {
                L2CGpCourseGetInfoRes obj = new L2CGpCourseGetInfoRes();

                ReadServerResponse(buffer, obj);
                obj.CourseInfo = ReadEntityList<CDataGPCourseInfo>(buffer);
                obj.Effects = ReadEntityList<CDataGPCourseEffectParam>(buffer);
                obj.Version = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
