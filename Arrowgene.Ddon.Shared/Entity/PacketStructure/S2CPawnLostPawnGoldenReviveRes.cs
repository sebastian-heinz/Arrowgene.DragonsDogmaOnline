using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnLostPawnGoldenReviveRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_LOST_PAWN_GOLDEN_REVIVE_RES;

        public uint PawnId { get; set; }
        public byte GP { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnLostPawnGoldenReviveRes>
        {
            public override void Write(IBuffer buffer, S2CPawnLostPawnGoldenReviveRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, obj.GP);
            }

            public override S2CPawnLostPawnGoldenReviveRes Read(IBuffer buffer)
            {
                S2CPawnLostPawnGoldenReviveRes obj = new S2CPawnLostPawnGoldenReviveRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.GP = ReadByte(buffer);
                return obj;
            }
        }
    }
}