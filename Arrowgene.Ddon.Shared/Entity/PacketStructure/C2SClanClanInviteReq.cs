using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanInviteReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_INVITE_REQ;

        public uint CharacterId { get; set; }

        public C2SClanClanInviteReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SClanClanInviteReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanInviteReq obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override C2SClanClanInviteReq Read(IBuffer buffer)
            {
                C2SClanClanInviteReq obj = new C2SClanClanInviteReq();
                obj.CharacterId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
