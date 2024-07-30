using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SGpGetGpPeriodReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_GP_GET_GP_PERIOD_REQ;

        public class Serializer : PacketEntitySerializer<C2SGpGetGpPeriodReq>
        {
            public override void Write(IBuffer buffer, C2SGpGetGpPeriodReq obj)
            {
            }

            public override C2SGpGetGpPeriodReq Read(IBuffer buffer)
            {
                C2SGpGetGpPeriodReq obj = new C2SGpGetGpPeriodReq();
                return obj;
            }
        }
    }
}
