using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartnerPawnSetRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PARTNER_PAWN_PARTNER_PAWN_SET_RES;

        public CDataPartnerPawnData PartnerInfo { get; set; } = new();

        public S2CPartnerPawnSetRes()
        {
            PartnerInfo = new();
        }

        public class Serializer : PacketEntitySerializer<S2CPartnerPawnSetRes>
        {
            public override void Write(IBuffer buffer, S2CPartnerPawnSetRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteEntity<CDataPartnerPawnData>(buffer, obj.PartnerInfo);
            }

            public override S2CPartnerPawnSetRes Read(IBuffer buffer)
            {
                S2CPartnerPawnSetRes obj = new S2CPartnerPawnSetRes();

                ReadServerResponse(buffer, obj);

                obj.PartnerInfo = ReadEntity<CDataPartnerPawnData>(buffer);

                return obj;
            }
        }
    }
}
