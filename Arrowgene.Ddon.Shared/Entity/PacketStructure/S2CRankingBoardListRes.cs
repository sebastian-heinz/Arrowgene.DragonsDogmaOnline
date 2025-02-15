using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CRankingBoardListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_RANKING_BOARD_LIST_RES;

        public List<CDataRankingBoard> RankingBoardList { get; set; } = new();

        public class Serializer : PacketEntitySerializer<S2CRankingBoardListRes>
        {
            public override void Write(IBuffer buffer, S2CRankingBoardListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.RankingBoardList);
            }

            public override S2CRankingBoardListRes Read(IBuffer buffer)
            {
                S2CRankingBoardListRes obj = new S2CRankingBoardListRes();
                ReadServerResponse(buffer, obj);
                obj.RankingBoardList = ReadEntityList<CDataRankingBoard>(buffer);
                return obj;
            }
        }
    }
}
