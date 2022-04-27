using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanPartnerPawnDataGetReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_PARTNER_PAWN_DATA_GET_REQ;

        public uint PawnId { get; set; }
        public byte Data0 { get; set; }
        public byte Data1 { get; set; }
        public byte Data2 { get; set; }

        public C2SClanClanPartnerPawnDataGetReq()
        {
            PawnId = 0;
            Data0 = 0;
            Data1 = 0;
            Data2 = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SClanClanPartnerPawnDataGetReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanPartnerPawnDataGetReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, obj.Data0);
                WriteByte(buffer, obj.Data1);
                WriteByte(buffer, obj.Data2);
            }

            public override C2SClanClanPartnerPawnDataGetReq Read(IBuffer buffer)
            {
                C2SClanClanPartnerPawnDataGetReq obj = new C2SClanClanPartnerPawnDataGetReq();
                obj.PawnId = ReadUInt32(buffer);
                obj.Data0 = ReadByte(buffer);
                obj.Data1 = ReadByte(buffer);
                obj.Data2 = ReadByte(buffer);
                return obj;
            }
        }
    }
}
