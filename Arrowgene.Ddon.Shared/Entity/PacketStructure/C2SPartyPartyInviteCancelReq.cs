using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPartyPartyInviteCancelReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PARTY_PARTY_INVITE_CANCEL_REQ;

        public C2SPartyPartyInviteCancelReq()
        {
            ServerId = 0;
            PartyId = 0;
        }

        public ushort ServerId { get; set; }
        public uint PartyId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPartyPartyInviteCancelReq>
        {
            public override void Write(IBuffer buffer, C2SPartyPartyInviteCancelReq obj)
            {
                WriteUInt16(buffer, obj.ServerId);
                WriteUInt32(buffer, obj.PartyId);
            }

            public override C2SPartyPartyInviteCancelReq Read(IBuffer buffer)
            {
                C2SPartyPartyInviteCancelReq obj = new C2SPartyPartyInviteCancelReq();
                obj.ServerId = ReadUInt16(buffer);
                obj.PartyId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
