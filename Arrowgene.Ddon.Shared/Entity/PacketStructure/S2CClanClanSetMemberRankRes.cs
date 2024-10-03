using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanSetMemberRankRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_SET_MEMBER_RANK_RES;

        public uint MemberId {  get; set; }
        public uint Rank { get; set; }

        public class Serializer : PacketEntitySerializer<S2CClanClanSetMemberRankRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanSetMemberRankRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.MemberId);
                WriteUInt32(buffer, obj.Rank);
            }

            public override S2CClanClanSetMemberRankRes Read(IBuffer buffer)
            {
                S2CClanClanSetMemberRankRes obj = new S2CClanClanSetMemberRankRes();
                ReadServerResponse(buffer, obj);
                obj.MemberId = ReadUInt32(buffer);
                obj.Rank = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
