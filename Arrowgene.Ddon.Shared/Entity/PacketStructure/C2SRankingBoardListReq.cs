using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SRankingBoardListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_RANKING_BOARD_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SRankingBoardListReq>
        {
            public override void Write(IBuffer buffer, C2SRankingBoardListReq obj)
            {
            }

            public override C2SRankingBoardListReq Read(IBuffer buffer)
            {
                C2SRankingBoardListReq obj = new C2SRankingBoardListReq();
                return obj;
            }
        }
    }
}
