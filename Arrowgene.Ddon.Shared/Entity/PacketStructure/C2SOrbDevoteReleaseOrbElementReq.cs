using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SOrbDevoteReleaseOrbElementReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ORB_DEVOTE_RELEASE_ORB_ELEMENT_REQ;

        public C2SOrbDevoteReleaseOrbElementReq()
        {
        }

        public UInt32 ElementId {  get; set; }

        public class Serializer : PacketEntitySerializer<C2SOrbDevoteReleaseOrbElementReq>
        {

            public override void Write(IBuffer buffer, C2SOrbDevoteReleaseOrbElementReq obj)
            {
                WriteUInt32(buffer, obj.ElementId);
            }

            public override C2SOrbDevoteReleaseOrbElementReq Read(IBuffer buffer)
            {
                C2SOrbDevoteReleaseOrbElementReq obj = new C2SOrbDevoteReleaseOrbElementReq();
                obj.ElementId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}
