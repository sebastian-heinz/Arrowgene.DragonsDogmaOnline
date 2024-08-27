using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnReturnRentedPawnReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_RETURN_RENTED_PAWN_REQ;

        public C2SPawnReturnRentedPawnReq()
        {
            PawnFeedbackList = new List<CDataPawnFeedback>();
        }

        public byte SlotNo {  get; set; }
        public List<CDataPawnFeedback> PawnFeedbackList {  get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnReturnRentedPawnReq>
        {
            public override void Write(IBuffer buffer, C2SPawnReturnRentedPawnReq obj)
            {
                WriteByte(buffer, obj.SlotNo);
                WriteEntityList(buffer, obj.PawnFeedbackList);
            }

            public override C2SPawnReturnRentedPawnReq Read(IBuffer buffer)
            {
                C2SPawnReturnRentedPawnReq obj = new C2SPawnReturnRentedPawnReq();
                obj.SlotNo = ReadByte(buffer);
                obj.PawnFeedbackList = ReadEntityList<CDataPawnFeedback>(buffer);
                return obj;
            }
        }
    }
}
