using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanSetMemberRankReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_SET_MEMBER_RANK_REQ;

        public uint CharacterId { get; set; }
        public uint Rank { get; set; }
        public uint Permission { get; set; }

        public class Serializer : PacketEntitySerializer<C2SClanClanSetMemberRankReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanSetMemberRankReq obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, obj.Rank);
                WriteUInt32(buffer, obj.Permission);
            }

            public override C2SClanClanSetMemberRankReq Read(IBuffer buffer)
            {
                C2SClanClanSetMemberRankReq obj = new C2SClanClanSetMemberRankReq();
                obj.CharacterId = ReadUInt32(buffer);
                obj.Rank = ReadUInt32(buffer);
                obj.Permission = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
