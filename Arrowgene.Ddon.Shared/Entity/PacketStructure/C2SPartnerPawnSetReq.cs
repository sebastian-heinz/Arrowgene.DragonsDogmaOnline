using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPartnerPawnSetReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PARTNER_PAWN_PARTNER_PAWN_SET_REQ;

        public uint PawnId;

        public C2SPartnerPawnSetReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SPartnerPawnSetReq>
        {
            public override void Write(IBuffer buffer, C2SPartnerPawnSetReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
            }

            public override C2SPartnerPawnSetReq Read(IBuffer buffer)
            {
                C2SPartnerPawnSetReq obj = new C2SPartnerPawnSetReq();

                obj.PawnId = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
