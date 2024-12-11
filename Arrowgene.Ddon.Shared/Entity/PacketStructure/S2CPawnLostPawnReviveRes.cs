using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnLostPawnReviveRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_LOST_PAWN_REVIVE_RES;

        public uint PawnId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnLostPawnReviveRes>
        {
            public override void Write(IBuffer buffer, S2CPawnLostPawnReviveRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
            }

            public override S2CPawnLostPawnReviveRes Read(IBuffer buffer)
            {
                S2CPawnLostPawnReviveRes obj = new S2CPawnLostPawnReviveRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}