using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnGetPawnTotalScoreInfoNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PAWN_GET_PAWN_TOTAL_SCORE_INFO_NTC;

        public S2CPawnGetPawnTotalScoreInfoNtc()
        {
            PawnTotalScore = new CDataPawnTotalScore();
        }

        public uint CharacterId { get; set; }
        public uint PawnId { get; set; }
        public CDataPawnTotalScore PawnTotalScore { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnGetPawnTotalScoreInfoNtc>
        {
            public override void Write(IBuffer buffer, S2CPawnGetPawnTotalScoreInfoNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntity<CDataPawnTotalScore>(buffer, obj.PawnTotalScore);
            }

            public override S2CPawnGetPawnTotalScoreInfoNtc Read(IBuffer buffer)
            {
                S2CPawnGetPawnTotalScoreInfoNtc obj = new S2CPawnGetPawnTotalScoreInfoNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.PawnId = ReadUInt32(buffer);
                obj.PawnTotalScore = ReadEntity<CDataPawnTotalScore>(buffer);
                return obj;
            }
        }

    }
}
