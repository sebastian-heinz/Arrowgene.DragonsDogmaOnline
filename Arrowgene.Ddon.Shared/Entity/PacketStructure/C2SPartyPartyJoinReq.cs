using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPartyPartyJoinReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PARTY_PARTY_JOIN_REQ;

        public uint PartyId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPartyPartyJoinReq>
        {
            public override void Write(IBuffer buffer, C2SPartyPartyJoinReq obj)
            {
                WriteUInt32(buffer, obj.PartyId);
            }

            public override C2SPartyPartyJoinReq Read(IBuffer buffer)
            {
                return new C2SPartyPartyJoinReq
                {
                    PartyId = ReadUInt32(buffer)
                };
            }
        }
    }
}