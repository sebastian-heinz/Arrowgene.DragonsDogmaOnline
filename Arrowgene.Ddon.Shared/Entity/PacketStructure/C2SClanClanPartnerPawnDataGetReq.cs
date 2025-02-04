using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanPartnerPawnDataGetReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_PARTNER_PAWN_DATA_GET_REQ;

        public uint PawnId { get; set; }

        public C2SClanClanPartnerPawnDataGetReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SClanClanPartnerPawnDataGetReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanPartnerPawnDataGetReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
            }

            public override C2SClanClanPartnerPawnDataGetReq Read(IBuffer buffer)
            {
                C2SClanClanPartnerPawnDataGetReq obj = new C2SClanClanPartnerPawnDataGetReq();
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
