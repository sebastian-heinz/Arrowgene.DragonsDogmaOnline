using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanInviteAcceptReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_INVITE_ACCEPT_REQ;

        public uint ClanId { get; set; }

        public C2SClanClanInviteAcceptReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SClanClanInviteAcceptReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanInviteAcceptReq obj)
            {
                WriteUInt32(buffer, obj.ClanId);
            }

            public override C2SClanClanInviteAcceptReq Read(IBuffer buffer)
            {
                C2SClanClanInviteAcceptReq obj = new C2SClanClanInviteAcceptReq();
                obj.ClanId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
