using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartnerPawnPresentForPartnerPawnRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PARTNER_PAWN_PRESENT_FOR_PARTNER_PAWN_RES;

        public S2CPartnerPawnPresentForPartnerPawnRes()
        {
            PartnerInfo = new CDataPartnerPawnData();
        }

        public CDataPartnerPawnData PartnerInfo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPartnerPawnPresentForPartnerPawnRes>
        {
            public override void Write(IBuffer buffer, S2CPartnerPawnPresentForPartnerPawnRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity(buffer, obj.PartnerInfo);
            }

            public override S2CPartnerPawnPresentForPartnerPawnRes Read(IBuffer buffer)
            {
                S2CPartnerPawnPresentForPartnerPawnRes obj = new S2CPartnerPawnPresentForPartnerPawnRes();
                ReadServerResponse(buffer, obj);
                obj.PartnerInfo = ReadEntity<CDataPartnerPawnData>(buffer);
                return obj;
            }
        }
    }
}
