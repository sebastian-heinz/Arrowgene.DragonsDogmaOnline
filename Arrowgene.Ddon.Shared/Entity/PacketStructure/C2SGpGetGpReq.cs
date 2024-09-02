using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SGpGetGpReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_GP_GET_GP_REQ;

        public class Serializer : PacketEntitySerializer<C2SGpGetGpReq>
        {
            public override void Write(IBuffer buffer, C2SGpGetGpReq obj)
            {
            }

            public override C2SGpGetGpReq Read(IBuffer buffer)
            {
                C2SGpGetGpReq obj = new C2SGpGetGpReq();
                return obj;
            }
        }

    }
}