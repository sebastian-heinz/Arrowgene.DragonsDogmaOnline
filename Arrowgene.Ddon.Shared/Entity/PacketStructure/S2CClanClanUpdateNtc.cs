using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanUpdateNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CLAN_CLAN_UPDATE_NTC;

        public S2CClanClanUpdateNtc()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CClanClanUpdateNtc>
        {
            public override void Write(IBuffer buffer, S2CClanClanUpdateNtc obj)
            {
            }

            public override S2CClanClanUpdateNtc Read(IBuffer buffer)
            {
                S2CClanClanUpdateNtc obj = new S2CClanClanUpdateNtc();
                return obj;
            }
        }
    }
}
