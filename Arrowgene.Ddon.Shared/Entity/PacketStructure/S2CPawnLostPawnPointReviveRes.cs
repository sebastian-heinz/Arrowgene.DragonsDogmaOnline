using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnLostPawnPointReviveRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_LOST_PAWN_POINT_REVIVE_RES;

        public uint PawnId { get; set; }
        public byte RevivePoint { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnLostPawnPointReviveRes>
        {
            public override void Write(IBuffer buffer, S2CPawnLostPawnPointReviveRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, obj.RevivePoint);
            }

            public override S2CPawnLostPawnPointReviveRes Read(IBuffer buffer)
            {
                S2CPawnLostPawnPointReviveRes obj = new S2CPawnLostPawnPointReviveRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.RevivePoint = ReadByte(buffer);
                return obj;
            }
        }
    }
}