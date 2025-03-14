using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanNegotiateMasterNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CLAN_CLAN_NEGOTIATE_MASTER_NTC;

        public uint ClanId { get; set; }
        public CDataClanMemberInfo MemberInfo { get; set; } = new();

        public class Serializer : PacketEntitySerializer<S2CClanClanNegotiateMasterNtc>
        {
            public override void Write(IBuffer buffer, S2CClanClanNegotiateMasterNtc obj)
            {
                WriteUInt32(buffer, obj.ClanId);
                WriteEntity<CDataClanMemberInfo>(buffer, obj.MemberInfo);
            }

            public override S2CClanClanNegotiateMasterNtc Read(IBuffer buffer)
            {
                S2CClanClanNegotiateMasterNtc obj = new S2CClanClanNegotiateMasterNtc();
                obj.ClanId = ReadUInt32(buffer);
                obj.MemberInfo = ReadEntity<CDataClanMemberInfo>(buffer);
                return obj;
            }
        }
    }
}
