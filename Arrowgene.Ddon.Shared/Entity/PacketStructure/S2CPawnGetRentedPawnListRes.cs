using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnGetRentedPawnListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_GET_RENTED_PAWN_LIST_RES;

        public S2CPawnGetRentedPawnListRes()
        {
            RentedPawnList = new List<CDataRentedPawnList>();
        }

        public List<CDataRentedPawnList> RentedPawnList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnGetRentedPawnListRes>
        {
            public override void Write(IBuffer buffer, S2CPawnGetRentedPawnListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.RentedPawnList);
            }

            public override S2CPawnGetRentedPawnListRes Read(IBuffer buffer)
            {
                S2CPawnGetRentedPawnListRes obj = new S2CPawnGetRentedPawnListRes();
                ReadServerResponse(buffer, obj);
                obj.RentedPawnList = ReadEntityList<CDataRentedPawnList>(buffer);
                return obj;
            }
        }
    }
}
