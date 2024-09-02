using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SOrbDevoteGetPawnReleaseOrbElementListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ORB_DEVOTE_GET_PAWN_RELEASE_ORB_ELEMENT_LIST_REQ;

        public C2SOrbDevoteGetPawnReleaseOrbElementListReq()
        {
        }

        public UInt32 PawnId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SOrbDevoteGetPawnReleaseOrbElementListReq>
        {

            public override void Write(IBuffer buffer, C2SOrbDevoteGetPawnReleaseOrbElementListReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
            }

            public override C2SOrbDevoteGetPawnReleaseOrbElementListReq Read(IBuffer buffer)
            {
                C2SOrbDevoteGetPawnReleaseOrbElementListReq obj = new C2SOrbDevoteGetPawnReleaseOrbElementListReq();
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
