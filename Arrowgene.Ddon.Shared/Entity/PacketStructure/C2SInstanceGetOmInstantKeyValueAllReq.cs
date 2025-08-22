using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstanceGetOmInstantKeyValueAllReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_GET_OM_INSTANT_KEY_VALUE_ALL_REQ;

        public class Serializer : PacketEntitySerializer<C2SInstanceGetOmInstantKeyValueAllReq>
        {
            public override void Write(IBuffer buffer, C2SInstanceGetOmInstantKeyValueAllReq obj)
            {
            }

            public override C2SInstanceGetOmInstantKeyValueAllReq Read(IBuffer buffer)
            {
                C2SInstanceGetOmInstantKeyValueAllReq obj = new C2SInstanceGetOmInstantKeyValueAllReq();
                return obj;
            }
        }

    }
}
