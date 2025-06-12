using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobOrbTreeReleaseJobOrbElementRes : ServerResponse
    {
        public S2CJobOrbTreeReleaseJobOrbElementRes()
        {
            TreeStatus = new CDataJobOrbTreeStatus();
        }

        public override PacketId Id => PacketId.S2C_JOB_ORB_TREE_RELEASE_JOB_ORB_ELEMENT_RES;

        public JobId JobId { get; set; }
        public uint RestBloodOrb { get; set; }
        public uint RestHighOrb { get; set; }
        public CDataJobOrbTreeStatus TreeStatus { get; set; }

        public class Serializer : PacketEntitySerializer<S2CJobOrbTreeReleaseJobOrbElementRes>
        {
            public override void Write(IBuffer buffer, S2CJobOrbTreeReleaseJobOrbElementRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, (byte) obj.JobId);
                WriteUInt32(buffer, obj.RestBloodOrb);
                WriteUInt32(buffer, obj.RestHighOrb);
                WriteEntity(buffer, obj.TreeStatus);
            }

            public override S2CJobOrbTreeReleaseJobOrbElementRes Read(IBuffer buffer)
            {
                S2CJobOrbTreeReleaseJobOrbElementRes obj = new S2CJobOrbTreeReleaseJobOrbElementRes();
                ReadServerResponse(buffer, obj);
                obj.JobId = (JobId) ReadByte(buffer);
                obj.RestBloodOrb = ReadUInt32(buffer);
                obj.RestHighOrb = ReadUInt32(buffer);
                obj.TreeStatus = ReadEntity<CDataJobOrbTreeStatus>(buffer);
                return obj;
            }
        }
    }
}
