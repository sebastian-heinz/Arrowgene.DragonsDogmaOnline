using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnGetPawnHistoryListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_GET_PAWN_HISTORY_LIST_RES;

        public S2CPawnGetPawnHistoryListRes()
        {
            PawnHistoryList = new List<CDataPawnHistory>();
        }

        public List<CDataPawnHistory> PawnHistoryList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnGetPawnHistoryListRes>
        {
            public override void Write(IBuffer buffer, S2CPawnGetPawnHistoryListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataPawnHistory>(buffer, obj.PawnHistoryList);
            }

            public override S2CPawnGetPawnHistoryListRes Read(IBuffer buffer)
            {
                S2CPawnGetPawnHistoryListRes obj = new S2CPawnGetPawnHistoryListRes();
                ReadServerResponse(buffer, obj);
                obj.PawnHistoryList = ReadEntityList<CDataPawnHistory>(buffer);
                return obj;
            }
        }
    }
}
