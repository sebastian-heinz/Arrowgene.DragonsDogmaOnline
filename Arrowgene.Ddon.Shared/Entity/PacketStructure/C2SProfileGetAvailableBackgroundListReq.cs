using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SProfileGetAvailableBackgroundListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PROFILE_GET_AVAILABLE_BACKGROUND_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SProfileGetAvailableBackgroundListReq>
        {
            public override void Write(IBuffer buffer, C2SProfileGetAvailableBackgroundListReq obj)
            {
            }

            public override C2SProfileGetAvailableBackgroundListReq Read(IBuffer buffer)
            {
                C2SProfileGetAvailableBackgroundListReq obj = new C2SProfileGetAvailableBackgroundListReq();
                return obj;
            }
        }
    }
}
