using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnGetMypawnListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_GET_MYPAWN_LIST_REQ;

        public class Serializer : EntitySerializer<C2SPawnGetMypawnListReq>
        {
            public override void Write(IBuffer buffer, C2SPawnGetMypawnListReq obj)
            {
            }

            public override C2SPawnGetMypawnListReq Read(IBuffer buffer)
            {
                return new C2SPawnGetMypawnListReq();
            }
        }
    }
}