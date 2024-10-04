using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanNegotiateMasterReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_NEGOTIATE_MASTER_REQ;

        public uint CharacterId { get; set; }

        public C2SClanClanNegotiateMasterReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SClanClanNegotiateMasterReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanNegotiateMasterReq obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override C2SClanClanNegotiateMasterReq Read(IBuffer buffer)
            {
                C2SClanClanNegotiateMasterReq obj = new C2SClanClanNegotiateMasterReq();
                obj.CharacterId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
