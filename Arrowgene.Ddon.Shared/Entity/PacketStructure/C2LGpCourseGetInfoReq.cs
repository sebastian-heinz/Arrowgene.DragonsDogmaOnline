using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2LGpCourseGetInfoReq : IPacketStructure
    {
        public C2LGpCourseGetInfoReq()
        {
        }

        public PacketId Id => PacketId.C2L_GP_COURSE_GET_INFO_REQ;

        public class Serializer : PacketEntitySerializer<C2LGpCourseGetInfoReq>
        {

            public override void Write(IBuffer buffer, C2LGpCourseGetInfoReq obj)
            {
                // WriteEntity(buffer, obj.CharacterInfo);
                // WriteUInt32(buffer, obj.WaitNum);
                // WriteByte(buffer, obj.RotationServerId);
            }

            public override C2LGpCourseGetInfoReq Read(IBuffer buffer)
            {
                // C2LCreateCharacterDataReq obj = new C2LCreateCharacterDataReq();
                // obj.CharacterInfo = ReadEntity<CDataCharacterInfo>(buffer);
                // obj.WaitNum = ReadUInt32(buffer);
                // obj.RotationServerId = ReadByte(buffer);
                // return obj;
                return new C2LGpCourseGetInfoReq();
            }
        }
    }
}
