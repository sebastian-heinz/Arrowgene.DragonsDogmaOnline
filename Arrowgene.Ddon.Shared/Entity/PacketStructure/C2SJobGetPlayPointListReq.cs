using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SJobGetPlayPointListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_JOB_GET_PLAY_POINT_LIST_REQ;

        public C2SJobGetPlayPointListReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SJobGetPlayPointListReq>
        {

            public override void Write(IBuffer buffer, C2SJobGetPlayPointListReq obj)
            {
            }

            public override C2SJobGetPlayPointListReq Read(IBuffer buffer)
            {
                return new C2SJobGetPlayPointListReq();
            }
        }
    }
}
