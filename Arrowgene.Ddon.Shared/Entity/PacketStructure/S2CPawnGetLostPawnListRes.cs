using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnGetLostPawnListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_GET_LOST_PAWN_LIST_RES;

        public S2CPawnGetLostPawnListRes()
        {
            LostPawnList = new List<CDataLostPawnList>();
        }

        public List<CDataLostPawnList> LostPawnList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnGetLostPawnListRes>
        {
            public override void Write(IBuffer buffer, S2CPawnGetLostPawnListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataLostPawnList>(buffer, obj.LostPawnList); 
            }

            public override S2CPawnGetLostPawnListRes Read(IBuffer buffer)
            {
                S2CPawnGetLostPawnListRes obj = new S2CPawnGetLostPawnListRes();
                ReadServerResponse(buffer, obj);
                obj.LostPawnList = ReadEntityList<CDataLostPawnList>(buffer);
                return obj;
            }
        }

    }
}
