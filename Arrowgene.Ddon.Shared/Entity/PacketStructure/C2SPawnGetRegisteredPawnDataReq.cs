using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnGetRegisteredPawnDataReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_GET_REGISTERED_PAWN_DATA_REQ;

        public int PawnId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnGetRegisteredPawnDataReq>
        {
            public override void Write(IBuffer buffer, C2SPawnGetRegisteredPawnDataReq obj)
            {
                WriteInt32(buffer, obj.PawnId);
            }

            public override C2SPawnGetRegisteredPawnDataReq Read(IBuffer buffer)
            {
                return new C2SPawnGetRegisteredPawnDataReq
                {
                    PawnId = ReadInt32(buffer)
                };
            }
        }
    }
}