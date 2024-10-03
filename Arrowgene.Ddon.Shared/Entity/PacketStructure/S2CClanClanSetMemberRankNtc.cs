using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanSetMemberRankNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CLAN_CLAN_SET_MEMBER_RANK_NTC;

        public uint ClanId { get; set; }
        public uint CharacterId { get; set; }
        public uint Rank { get; set; }
        public uint Permission { get; set; }

        public class Serializer : PacketEntitySerializer<S2CClanClanSetMemberRankNtc>
        {
            public override void Write(IBuffer buffer, S2CClanClanSetMemberRankNtc obj)
            {
                WriteUInt32(buffer, obj.ClanId);
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, obj.Rank);
                WriteUInt32(buffer, obj.Permission);
            }

            public override S2CClanClanSetMemberRankNtc Read(IBuffer buffer)
            {
                S2CClanClanSetMemberRankNtc obj = new S2CClanClanSetMemberRankNtc();
                obj.ClanId = ReadUInt32(buffer);
                obj.CharacterId = ReadUInt32(buffer);
                obj.Rank = ReadUInt32(buffer);
                obj.Permission = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
