using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobGetPlayPointListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_JOB_GET_PLAY_POINT_LIST_RES;

        public S2CJobGetPlayPointListRes()
        {
            PlayPointList = new List<CDataJobPlayPoint>();
        }

        public List<CDataJobPlayPoint> PlayPointList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CJobGetPlayPointListRes>
        {
            public override void Write(IBuffer buffer, S2CJobGetPlayPointListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataJobPlayPoint>(buffer, obj.PlayPointList);
            }

            public override S2CJobGetPlayPointListRes Read(IBuffer buffer)
            {
                S2CJobGetPlayPointListRes obj = new S2CJobGetPlayPointListRes();
                ReadServerResponse(buffer, obj);
                obj.PlayPointList = ReadEntityList<CDataJobPlayPoint>(buffer);
                return obj;
            }
        }
    }
}
