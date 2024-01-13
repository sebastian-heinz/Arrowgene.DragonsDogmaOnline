using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnPawnLostRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_PAWN_LOST_RES;

        public S2CPawnPawnLostRes()
        {
            PawnName = string.Empty;
        }

        public uint PawnId { get; set; }
        public string PawnName { get; set; }
        public bool IsLost { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnPawnLostRes>
        {
            public override void Write(IBuffer buffer, S2CPawnPawnLostRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteMtString(buffer, obj.PawnName);
                WriteBool(buffer, obj.IsLost);
            }

            public override S2CPawnPawnLostRes Read(IBuffer buffer)
            {
                S2CPawnPawnLostRes obj = new S2CPawnPawnLostRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.PawnName = ReadMtString(buffer);
                obj.IsLost = ReadBool(buffer);
                return obj;
            }
        }
    }
}