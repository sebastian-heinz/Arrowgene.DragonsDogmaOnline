using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnGetRegisteredPawnDataRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_GET_REGISTERED_PAWN_DATA_RES;

        public S2CPawnGetRegisteredPawnDataRes()
        {
            PawnInfo = new CDataPawnInfo();
        }

        public uint PawnId { get; set; }
        public CDataPawnInfo PawnInfo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnGetRegisteredPawnDataRes>
        {
            public override void Write(IBuffer buffer, S2CPawnGetRegisteredPawnDataRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntity<CDataPawnInfo>(buffer, obj.PawnInfo);
            }

            public override S2CPawnGetRegisteredPawnDataRes Read(IBuffer buffer)
            {
                S2CPawnGetRegisteredPawnDataRes obj = new S2CPawnGetRegisteredPawnDataRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.PawnInfo = ReadEntity<CDataPawnInfo>(buffer);
                return obj;
            }
        }
    }
}