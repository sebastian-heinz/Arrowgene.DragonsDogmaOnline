using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanPartnerPawnDataGetRes : ServerResponse
    {
        public S2CClanClanPartnerPawnDataGetRes()
        {
            PawnInfo = new();
        }

        public uint PawnId { get; set; }
        public CDataNoraPawnInfo PawnInfo { get; set; }

        public override PacketId Id => PacketId.S2C_CLAN_CLAN_PARTNER_PAWN_DATA_GET_RES;

        public class Serializer : PacketEntitySerializer<S2CClanClanPartnerPawnDataGetRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanPartnerPawnDataGetRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntity<CDataNoraPawnInfo>(buffer, obj.PawnInfo);
            }

            public override S2CClanClanPartnerPawnDataGetRes Read(IBuffer buffer)
            {
                S2CClanClanPartnerPawnDataGetRes obj = new S2CClanClanPartnerPawnDataGetRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.PawnInfo = ReadEntity<CDataNoraPawnInfo>(buffer);
                return obj;
            }
        }
    }
}
