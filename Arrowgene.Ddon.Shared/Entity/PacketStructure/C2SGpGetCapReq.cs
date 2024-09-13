using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure;

public class C2SGpGetCapReq : IPacketStructure
{
    public PacketId Id => PacketId.C2S_GP_GET_CAP_REQ;

    public class Serializer : PacketEntitySerializer<C2SGpGetCapReq>
    {
        public override void Write(IBuffer buffer, C2SGpGetCapReq obj)
        {
        }

        public override C2SGpGetCapReq Read(IBuffer buffer)
        {
            var obj = new C2SGpGetCapReq();

            return obj;
        }
    }
}
