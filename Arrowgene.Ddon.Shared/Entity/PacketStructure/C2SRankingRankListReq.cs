using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SRankingRankListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_RANKING_RANK_LIST_REQ;

        public uint BoardId { get; set; }
        public uint Rank { get; set; }
        public byte Num { get; set; }

        public class Serializer : PacketEntitySerializer<C2SRankingRankListReq>
        {
            public override void Write(IBuffer buffer, C2SRankingRankListReq obj)
            {
                WriteUInt32(buffer, obj.BoardId);
                WriteUInt32(buffer, obj.Rank);
                WriteByte(buffer, obj.Num);
            }

            public override C2SRankingRankListReq Read(IBuffer buffer)
            {
                C2SRankingRankListReq obj = new C2SRankingRankListReq();
                obj.BoardId = ReadUInt32(buffer);
                obj.Rank = ReadUInt32(buffer);
                obj.Num = ReadByte(buffer);
                return obj;
            }
        }
    }
}
