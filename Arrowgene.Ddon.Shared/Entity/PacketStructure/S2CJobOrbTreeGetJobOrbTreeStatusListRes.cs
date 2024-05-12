using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobOrbTreeGetJobOrbTreeStatusListRes : ServerResponse
    {
        public S2CJobOrbTreeGetJobOrbTreeStatusListRes()
        {
            TreeStatusList = new List<CDataJobOrbTreeStatus>();
        }
        public override PacketId Id => PacketId.S2C_JOB_ORB_TREE_GET_JOB_ORB_TREE_STATUS_LIST_RES;

        public List<CDataJobOrbTreeStatus> TreeStatusList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CJobOrbTreeGetJobOrbTreeStatusListRes>
        {
            public override void Write(IBuffer buffer, S2CJobOrbTreeGetJobOrbTreeStatusListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataJobOrbTreeStatus>(buffer, obj.TreeStatusList);
            }

            public override S2CJobOrbTreeGetJobOrbTreeStatusListRes Read(IBuffer buffer)
            {
                S2CJobOrbTreeGetJobOrbTreeStatusListRes obj = new S2CJobOrbTreeGetJobOrbTreeStatusListRes();
                ReadServerResponse(buffer, obj);
                obj.TreeStatusList = ReadEntityList<CDataJobOrbTreeStatus>(buffer);
                return obj;
            }
        }
    }
}
