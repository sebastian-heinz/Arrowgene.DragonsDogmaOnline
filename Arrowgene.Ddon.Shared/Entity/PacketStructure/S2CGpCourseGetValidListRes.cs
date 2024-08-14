using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure;

public class S2CGpCourseGetValidListRes : ServerResponse
{
    public S2CGpCourseGetValidListRes()
    {
        Items = new List<CDataGPCourseValid>();
    }

    public override PacketId Id => PacketId.S2C_GP_GP_COURSE_GET_VALID_LIST_RES;

    public List<CDataGPCourseValid> Items { get; set; }

    public class Serializer : PacketEntitySerializer<S2CGpCourseGetValidListRes>
    {
        public override void Write(IBuffer buffer, S2CGpCourseGetValidListRes obj)
        {
            WriteServerResponse(buffer, obj);

            WriteEntityList(buffer, obj.Items);
        }

        public override S2CGpCourseGetValidListRes Read(IBuffer buffer)
        {
            var obj = new S2CGpCourseGetValidListRes();

            ReadServerResponse(buffer, obj);

            obj.Items = ReadEntityList<CDataGPCourseValid>(buffer);

            return obj;
        }
    }
}
