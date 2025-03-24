using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnGetMyPawnDataReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_GET_MYPAWN_DATA_REQ;
        
        public byte SlotNo { get; set; }
        public int PawnId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnGetMyPawnDataReq>
        {
            public override void Write(IBuffer buffer, C2SPawnGetMyPawnDataReq obj)
            {
                WriteByte(buffer, obj.SlotNo);
                WriteInt32(buffer, obj.PawnId);
            }

            public override C2SPawnGetMyPawnDataReq Read(IBuffer buffer)
            {
                C2SPawnGetMyPawnDataReq obj = new C2SPawnGetMyPawnDataReq();
                obj.SlotNo = ReadByte(buffer);
                obj.PawnId = ReadInt32(buffer);
                return obj;
            }
        }
    }
}
