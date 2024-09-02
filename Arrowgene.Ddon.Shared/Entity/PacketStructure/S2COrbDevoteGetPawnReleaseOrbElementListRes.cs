using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2COrbDevoteGetPawnReleaseOrbElementListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ORB_DEVOTE_GET_PAWN_RELEASE_ORB_ELEMENT_LIST_RES;

        public S2COrbDevoteGetPawnReleaseOrbElementListRes()
        {
            OrbElementList = new List<CDataReleaseOrbElement>();
        }

        public UInt32 PawnId { get; set; }

        public List<CDataReleaseOrbElement> OrbElementList { get; set; }

        public class Serializer : PacketEntitySerializer<S2COrbDevoteGetPawnReleaseOrbElementListRes>
        {
            public override void Write(IBuffer buffer, S2COrbDevoteGetPawnReleaseOrbElementListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList<CDataReleaseOrbElement>(buffer, obj.OrbElementList);
            }

            public override S2COrbDevoteGetPawnReleaseOrbElementListRes Read(IBuffer buffer)
            {
                S2COrbDevoteGetPawnReleaseOrbElementListRes obj = new S2COrbDevoteGetPawnReleaseOrbElementListRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.OrbElementList = ReadEntityList<CDataReleaseOrbElement>(buffer);
                return obj;
            }
        }
    }
}

