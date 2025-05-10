using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SJobOrbTreeReleaseJobOrbElementReq : IPacketStructure
    {
        public C2SJobOrbTreeReleaseJobOrbElementReq()
        {
        }
        public PacketId Id => PacketId.C2S_JOB_ORB_TREE_RELEASE_JOB_ORB_ELEMENT_REQ;

        public uint Unk0 { get; set; }
        public uint ElementId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SJobOrbTreeReleaseJobOrbElementReq>
        {
            public override void Write(IBuffer buffer, C2SJobOrbTreeReleaseJobOrbElementReq obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.ElementId);
            }

            public override C2SJobOrbTreeReleaseJobOrbElementReq Read(IBuffer buffer)
            {
                C2SJobOrbTreeReleaseJobOrbElementReq obj = new C2SJobOrbTreeReleaseJobOrbElementReq();
                obj.Unk0 = ReadUInt32(buffer);
                obj.ElementId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

