using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SJobOrbTreeGetJobOrbTreeStatusListReq : IPacketStructure
    {
        public C2SJobOrbTreeGetJobOrbTreeStatusListReq()
        {
        }
        public PacketId Id => PacketId.C2S_JOB_ORB_TREE_GET_JOB_ORB_TREE_STATUS_LIST_REQ;

        public uint Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<C2SJobOrbTreeGetJobOrbTreeStatusListReq>
        {
            public override void Write(IBuffer buffer, C2SJobOrbTreeGetJobOrbTreeStatusListReq obj)
            {
                WriteUInt32(buffer, obj.Unk0);
            }

            public override C2SJobOrbTreeGetJobOrbTreeStatusListReq Read(IBuffer buffer)
            {
                C2SJobOrbTreeGetJobOrbTreeStatusListReq obj = new C2SJobOrbTreeGetJobOrbTreeStatusListReq();
                obj.Unk0 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
