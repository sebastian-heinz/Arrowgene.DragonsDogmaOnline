using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnGetPartyPawnDataRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_GET_PARTY_PAWN_DATA_RES;

        public S2CPawnGetPartyPawnDataRes()
        {
            PawnInfo = new CDataPawnInfo();
        }

        public uint CharacterId { get; set; }
        public uint PawnId { get; set; }
        public CDataPawnInfo PawnInfo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnGetPartyPawnDataRes>
        {
            public override void Write(IBuffer buffer, S2CPawnGetPartyPawnDataRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntity<CDataPawnInfo>(buffer, obj.PawnInfo);
            }

            public override S2CPawnGetPartyPawnDataRes Read(IBuffer buffer)
            {
                S2CPawnGetPartyPawnDataRes obj = new S2CPawnGetPartyPawnDataRes();
                ReadServerResponse(buffer, obj);
                obj.CharacterId = ReadUInt32(buffer);
                obj.PawnId = ReadUInt32(buffer);
                obj.PawnInfo = ReadEntity<CDataPawnInfo>(buffer);
                return obj;
            }
        }
    }
}