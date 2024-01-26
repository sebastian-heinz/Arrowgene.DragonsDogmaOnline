using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnPawnLostReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_PAWN_LOST_REQ;

        public uint PawnId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnPawnLostReq>
        {
            public override void Write(IBuffer buffer, C2SPawnPawnLostReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
            }

            public override C2SPawnPawnLostReq Read(IBuffer buffer)
            {
                C2SPawnPawnLostReq obj = new C2SPawnPawnLostReq();
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}