using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnGetMyPawnListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_GET_MYPAWN_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SPawnGetMyPawnListReq>
        {
            public override void Write(IBuffer buffer, C2SPawnGetMyPawnListReq obj)
            {
            }

            public override C2SPawnGetMyPawnListReq Read(IBuffer buffer)
            {
                return new C2SPawnGetMyPawnListReq();
            }
        }
    }
}