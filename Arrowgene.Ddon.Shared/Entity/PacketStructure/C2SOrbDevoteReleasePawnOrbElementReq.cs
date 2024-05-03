using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SOrbDevoteReleasePawnOrbElementReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ORB_DEVOTE_RELEASE_PAWN_ORB_ELEMENT_REQ;

        public C2SOrbDevoteReleasePawnOrbElementReq()
        {
        }

        public UInt32 PawnId {  get; set; }
        public UInt32 ElementId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SOrbDevoteReleasePawnOrbElementReq>
        {

            public override void Write(IBuffer buffer, C2SOrbDevoteReleasePawnOrbElementReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteUInt32(buffer, obj.ElementId);
            }

            public override C2SOrbDevoteReleasePawnOrbElementReq Read(IBuffer buffer)
            {
                C2SOrbDevoteReleasePawnOrbElementReq obj = new C2SOrbDevoteReleasePawnOrbElementReq();
                obj.PawnId = ReadUInt32(buffer);
                obj.ElementId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}
