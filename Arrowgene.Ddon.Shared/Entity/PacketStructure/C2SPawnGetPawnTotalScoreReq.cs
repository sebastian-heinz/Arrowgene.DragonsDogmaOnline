using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnGetPawnTotalScoreReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_GET_PAWN_TOTAL_SCORE_REQ;

        public uint PawnId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnGetPawnTotalScoreReq>
        {
            public override void Write(IBuffer buffer, C2SPawnGetPawnTotalScoreReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
            }
        
            public override C2SPawnGetPawnTotalScoreReq Read(IBuffer buffer)
            {
                C2SPawnGetPawnTotalScoreReq obj = new C2SPawnGetPawnTotalScoreReq();
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}