using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanLeaveMemberNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CLAN_CLAN_LEAVE_MEMBER_NTC;

        public S2CClanClanLeaveMemberNtc()
        {
            CharacterListElement = new CDataCharacterListElement();
        }

        public uint ClanId {  get; set; }
        public CDataCharacterListElement CharacterListElement { get; set; }

        public class Serializer : PacketEntitySerializer<S2CClanClanLeaveMemberNtc>
        {
            public override void Write(IBuffer buffer, S2CClanClanLeaveMemberNtc obj)
            {
                WriteUInt32(buffer, obj.ClanId);
                WriteEntity<CDataCharacterListElement>(buffer, obj.CharacterListElement);
            }

            public override S2CClanClanLeaveMemberNtc Read(IBuffer buffer)
            {
                S2CClanClanLeaveMemberNtc obj = new S2CClanClanLeaveMemberNtc();
                obj.ClanId = ReadUInt32(buffer);
                obj.CharacterListElement = ReadEntity<CDataCharacterListElement>(buffer);
                return obj;
            }
        }
    }
}
