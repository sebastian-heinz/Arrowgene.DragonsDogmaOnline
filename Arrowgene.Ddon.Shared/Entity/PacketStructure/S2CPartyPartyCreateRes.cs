using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyCreateRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PARTY_PARTY_CREATE_RES;

        public uint PartyId { get; set; }
        public ulong ContentNumber { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPartyPartyCreateRes>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyCreateRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PartyId);
                WriteUInt64(buffer, obj.ContentNumber);
            }

            public override S2CPartyPartyCreateRes Read(IBuffer buffer)
            {
                S2CPartyPartyCreateRes obj = new S2CPartyPartyCreateRes();
                ReadServerResponse(buffer, obj);
                obj.PartyId = ReadUInt32(buffer);
                obj.ContentNumber = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}
