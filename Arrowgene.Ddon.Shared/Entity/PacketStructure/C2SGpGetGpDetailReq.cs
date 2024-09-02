using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SGpGetGpDetailReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_GP_GET_GP_DETAIL_REQ;

        public bool IsAll { get; set; }

        public class Serializer : PacketEntitySerializer<C2SGpGetGpDetailReq>
        {
            public override void Write(IBuffer buffer, C2SGpGetGpDetailReq obj)
            {
                WriteBool(buffer, obj.IsAll);
            }

            public override C2SGpGetGpDetailReq Read(IBuffer buffer)
            {
                C2SGpGetGpDetailReq obj = new C2SGpGetGpDetailReq();
                obj.IsAll = ReadBool(buffer);
                return obj;
            }
        }
    }
}
