using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnGetOfficialPawnListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_GET_OFFICIAL_PAWN_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SPawnGetOfficialPawnListReq>
        {
            public override void Write(IBuffer buffer, C2SPawnGetOfficialPawnListReq obj)
            {
            }

            public override C2SPawnGetOfficialPawnListReq Read(IBuffer buffer)
            {
                return new C2SPawnGetOfficialPawnListReq();
            }
        }
    }
}
