using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnGetPawnTotalScoreRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_GET_PAWN_TOTAL_SCORE_RES;

        public S2CPawnGetPawnTotalScoreRes()
        {
            PawnTotalScore = new CDataPawnTotalScore();
        }

        public uint PawnId { get; set; }
        public CDataPawnTotalScore PawnTotalScore { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnGetPawnTotalScoreRes>
        {
            public override void Write(IBuffer buffer, S2CPawnGetPawnTotalScoreRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntity<CDataPawnTotalScore>(buffer, obj.PawnTotalScore);
            }
        
            public override S2CPawnGetPawnTotalScoreRes Read(IBuffer buffer)
            {
                S2CPawnGetPawnTotalScoreRes obj = new S2CPawnGetPawnTotalScoreRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.PawnTotalScore = ReadEntity<CDataPawnTotalScore>(buffer);
                return obj;
            }
        }
    }
}