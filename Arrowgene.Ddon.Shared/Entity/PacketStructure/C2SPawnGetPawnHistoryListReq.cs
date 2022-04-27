using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnGetPawnHistoryListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_GET_PAWN_HISTORY_LIST_REQ;

        public uint PawnId { get; set; }

        public C2SPawnGetPawnHistoryListReq()
        {
            PawnId = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SPawnGetPawnHistoryListReq>
        {
            public override void Write(IBuffer buffer, C2SPawnGetPawnHistoryListReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
            }

            public override C2SPawnGetPawnHistoryListReq Read(IBuffer buffer)
            {
                C2SPawnGetPawnHistoryListReq obj = new C2SPawnGetPawnHistoryListReq();
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
