using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnGetLostPawnListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_GET_LOST_PAWN_LIST_REQ;

        public C2SPawnGetLostPawnListReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SPawnGetLostPawnListReq>
        {
            public override void Write(IBuffer buffer, C2SPawnGetLostPawnListReq obj)
            {
            }

            public override C2SPawnGetLostPawnListReq Read(IBuffer buffer)
            {
                return new C2SPawnGetLostPawnListReq();
            }
        }
    }
}
