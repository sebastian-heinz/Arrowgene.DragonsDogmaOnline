using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnGetRentedPawnDataRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_GET_RENTED_PAWN_DATA_RES;

        public S2CPawnGetRentedPawnDataRes()
        {
            PawnInfo = new CDataPawnInfo();
        }

        public uint PawnId {  get; set; }
        public CDataPawnInfo PawnInfo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnGetRentedPawnDataRes>
        {
            public override void Write(IBuffer buffer, S2CPawnGetRentedPawnDataRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntity(buffer, obj.PawnInfo);
            }

            public override S2CPawnGetRentedPawnDataRes Read(IBuffer buffer)
            {
                S2CPawnGetRentedPawnDataRes obj = new S2CPawnGetRentedPawnDataRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.PawnInfo = ReadEntity<CDataPawnInfo>(buffer);
                return obj;
            }
        }
    }
}
