using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyJoinRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PARTY_PARTY_JOIN_RES;

        public byte MemberIndex { get; set; }
        public ulong ContentNumber { get; set; }
        public uint PartyId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPartyPartyJoinRes>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyJoinRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, obj.MemberIndex);
                WriteUInt64(buffer, obj.ContentNumber);
                WriteUInt32(buffer, obj.PartyId);
            }

            public override S2CPartyPartyJoinRes Read(IBuffer buffer)
            {
                S2CPartyPartyJoinRes obj = new S2CPartyPartyJoinRes();
                ReadServerResponse(buffer, obj);
                obj.MemberIndex = ReadByte(buffer);
                obj.ContentNumber = ReadUInt64(buffer);
                obj.PartyId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}