using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGetFreeRentalPawnListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_GET_FREE_RENTAL_PAWN_LIST_RES;

        public List<CDataFreeRentalPawnList> FreeRentalPawnList { get; set; }

        public S2CGetFreeRentalPawnListRes()
        {
            FreeRentalPawnList = new List<CDataFreeRentalPawnList>();
        }

        public class Serializer : PacketEntitySerializer<S2CGetFreeRentalPawnListRes>
        {
            public override void Write(IBuffer buffer, S2CGetFreeRentalPawnListRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteEntityList<CDataFreeRentalPawnList>(buffer, obj.FreeRentalPawnList);
            }

            public override S2CGetFreeRentalPawnListRes Read(IBuffer buffer)
            {
                S2CGetFreeRentalPawnListRes obj = new S2CGetFreeRentalPawnListRes();

                ReadServerResponse(buffer, obj);

                obj.FreeRentalPawnList = ReadEntityList<CDataFreeRentalPawnList>(buffer);

                return obj;
            }
        }
    }
}
