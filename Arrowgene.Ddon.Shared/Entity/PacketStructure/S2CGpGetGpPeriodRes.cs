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
    public class S2CGpGetGpPeriodRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_GP_GET_GP_PERIOD_RES;

        public S2CGpGetGpPeriodRes()
        {
            GPPeriodList = new List<CDataGPPeriod>();
        }

        public List<CDataGPPeriod> GPPeriodList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CGpGetGpPeriodRes>
        {
            public override void Write(IBuffer buffer, S2CGpGetGpPeriodRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataGPPeriod>(buffer, obj.GPPeriodList);
            }

            public override S2CGpGetGpPeriodRes Read(IBuffer buffer)
            {
                S2CGpGetGpPeriodRes obj = new S2CGpGetGpPeriodRes();
                ReadServerResponse(buffer, obj);
                obj.GPPeriodList = ReadEntityList<CDataGPPeriod>(buffer);
                return obj;
            }
        }
    }
}
