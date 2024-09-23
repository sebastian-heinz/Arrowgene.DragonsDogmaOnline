using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanGetMemberListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_GET_MEMBER_LIST_REQ;

        public uint ClanId { get; set; }

        public C2SClanClanGetMemberListReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SClanClanGetMemberListReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanGetMemberListReq obj)
            {
                WriteUInt32(buffer, obj.ClanId);
            }

            public override C2SClanClanGetMemberListReq Read(IBuffer buffer)
            {
                C2SClanClanGetMemberListReq obj = new C2SClanClanGetMemberListReq();
                obj.ClanId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
