using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobUpdateExpModeRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_JOB_UPDATE_EXP_MODE_RES;

        public List<CDataJobPlayPoint> PlayPointList { get; set; } = new();

        public class Serializer : PacketEntitySerializer<S2CJobUpdateExpModeRes>
        {
            public override void Write(IBuffer buffer, S2CJobUpdateExpModeRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataJobPlayPoint>(buffer, obj.PlayPointList);
            }
        
            public override S2CJobUpdateExpModeRes Read(IBuffer buffer)
            {
                S2CJobUpdateExpModeRes obj = new S2CJobUpdateExpModeRes();
                ReadServerResponse(buffer, obj);
                obj.PlayPointList = ReadEntityList<CDataJobPlayPoint>(buffer);
                return obj;
            }
        }
    }
}
