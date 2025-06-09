using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListReq : IPacketStructure
    {
        public C2SJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListReq()
        {
        }
        public PacketId Id => PacketId.C2S_JOB_ORB_TREE_GET_ALL_JOB_ORB_ELEMENT_LIST_REQ;

        public OrbTreeType OrbTreeType { get; set; }
        public JobId JobId {  get; set; }

        public class Serializer : PacketEntitySerializer<C2SJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListReq>
        {
            public override void Write(IBuffer buffer, C2SJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListReq obj)
            {
                WriteUInt32(buffer, (uint) obj.OrbTreeType);
                WriteByte(buffer, (byte) obj.JobId);
            }

            public override C2SJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListReq Read(IBuffer buffer)
            {
                C2SJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListReq obj = new C2SJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListReq();
                obj.OrbTreeType = (OrbTreeType) ReadUInt32(buffer);
                obj.JobId = (JobId) ReadByte(buffer);
                return obj;
            }
        }
    }
}
