using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnGetMypawnDataReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_GET_MYPAWN_DATA_REQ;
        
        public byte SlotNo { get; set; }
        public int PawnId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnGetMypawnDataReq>
        {
            public override void Write(IBuffer buffer, C2SPawnGetMypawnDataReq obj)
            {
                WriteByte(buffer, obj.SlotNo);
                WriteInt32(buffer, obj.PawnId);
            }

            public override C2SPawnGetMypawnDataReq Read(IBuffer buffer)
            {
                C2SPawnGetMypawnDataReq obj = new C2SPawnGetMypawnDataReq();
                obj.SlotNo = ReadByte(buffer);
                obj.PawnId = ReadInt32(buffer);
                return obj;
            }
        }
    }
}
