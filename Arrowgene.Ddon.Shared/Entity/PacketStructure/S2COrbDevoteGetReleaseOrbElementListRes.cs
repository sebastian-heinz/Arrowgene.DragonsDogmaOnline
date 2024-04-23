using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2COrbDevoteGetReleaseOrbElementListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ORB_DEVOTE_GET_RELEASE_ORB_ELEMENT_LIST_RES;

        public S2COrbDevoteGetReleaseOrbElementListRes()
        {
            OrbElementList = new List<CDataReleaseOrbElement>();
        }

        public List<CDataReleaseOrbElement> OrbElementList {  get; set; }

        public class Serializer : PacketEntitySerializer<S2COrbDevoteGetReleaseOrbElementListRes>
        {
            public override void Write(IBuffer buffer, S2COrbDevoteGetReleaseOrbElementListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataReleaseOrbElement>(buffer, obj.OrbElementList);
            }

            public override S2COrbDevoteGetReleaseOrbElementListRes Read(IBuffer buffer)
            {
                S2COrbDevoteGetReleaseOrbElementListRes obj = new S2COrbDevoteGetReleaseOrbElementListRes();
                ReadServerResponse(buffer, obj);
                obj.OrbElementList = ReadEntityList<CDataReleaseOrbElement>(buffer);
                return obj;
            }
        }

    }
}
