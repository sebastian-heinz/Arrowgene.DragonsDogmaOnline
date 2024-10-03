using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanExpelMemberReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_EXPEL_MEMBER_REQ;

        public uint CharacterId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SClanClanExpelMemberReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanExpelMemberReq obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override C2SClanClanExpelMemberReq Read(IBuffer buffer)
            {
                C2SClanClanExpelMemberReq obj = new C2SClanClanExpelMemberReq();
                obj.CharacterId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
