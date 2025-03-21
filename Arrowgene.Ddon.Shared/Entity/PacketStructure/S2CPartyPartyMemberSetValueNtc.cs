using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyMemberSetValueNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PARTY_PARTY_MEMBER_SET_VALUE_NTC;

        public uint CharacterId { get; set; }
        public byte Index { get; set; }
        public byte Value { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPartyPartyMemberSetValueNtc>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyMemberSetValueNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteByte(buffer, obj.Index);
                WriteByte(buffer, obj.Value);
            }

            public override S2CPartyPartyMemberSetValueNtc Read(IBuffer buffer)
            {
                S2CPartyPartyMemberSetValueNtc obj = new S2CPartyPartyMemberSetValueNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.Index = ReadByte(buffer);
                obj.Value = ReadByte(buffer);
                return obj;
            }
        }
    }
}
