using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnGetNoraPawnListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_GET_NORA_PAWN_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SPawnGetNoraPawnListReq>
        {
            public override void Write(IBuffer buffer, C2SPawnGetNoraPawnListReq obj)
            {
            }

            public override C2SPawnGetNoraPawnListReq Read(IBuffer buffer)
            {
                return new C2SPawnGetNoraPawnListReq();
            }
        }
    }
}
