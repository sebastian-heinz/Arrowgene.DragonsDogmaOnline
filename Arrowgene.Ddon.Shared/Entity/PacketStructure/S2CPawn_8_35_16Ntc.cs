using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawn_8_35_16Ntc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PAWN_8_35_16_NTC;

        public S2CPawn_8_35_16Ntc()
        {
            PawnTotalScore = new CDataPawnTotalScore();
        }

        public uint CharacterId { get; set; }
        public uint PawnId { get; set; }
        public CDataPawnTotalScore PawnTotalScore { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawn_8_35_16Ntc>
        {
            public override void Write(IBuffer buffer, S2CPawn_8_35_16Ntc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntity<CDataPawnTotalScore>(buffer, obj.PawnTotalScore);
            }

            public override S2CPawn_8_35_16Ntc Read(IBuffer buffer)
            {
                S2CPawn_8_35_16Ntc obj = new S2CPawn_8_35_16Ntc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.PawnId = ReadUInt32(buffer);
                obj.PawnTotalScore = ReadEntity<CDataPawnTotalScore>(buffer);
                return obj;
            }
        }

    }
}