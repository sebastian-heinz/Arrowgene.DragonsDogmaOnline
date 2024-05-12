using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPartyPartyChangeLeaderReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PARTY_PARTY_CHANGE_LEADER_REQ;

        public C2SPartyPartyChangeLeaderReq()
        {
            CharacterId = 0;
        }

        public uint CharacterId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPartyPartyChangeLeaderReq>
        {
            public override void Write(IBuffer buffer, C2SPartyPartyChangeLeaderReq obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override C2SPartyPartyChangeLeaderReq Read(IBuffer buffer)
            {
                C2SPartyPartyChangeLeaderReq obj = new C2SPartyPartyChangeLeaderReq();
                obj.CharacterId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
