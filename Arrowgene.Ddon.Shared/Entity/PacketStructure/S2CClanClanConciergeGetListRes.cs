using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanConciergeGetListRes : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CLAN_CLAN_CONCIERGE_GET_LIST_RES;
        
        public class Serializer : PacketEntitySerializer<S2CClanClanConciergeGetListRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanConciergeGetListRes obj)
            {
                WriteByteArray(buffer, obj.ConciergeData);
            }

            public override S2CClanClanConciergeGetListRes Read(IBuffer buffer)
            {
                S2CClanClanConciergeGetListRes obj = new S2CClanClanConciergeGetListRes();
                return obj;
            }
        }

        private readonly byte[] ConciergeData = 
        {
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x3,0x0, 0x0, 0x4, 0x14,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x4, 0x13, 0x0, 0x0, 0x0, 0x0,0x0, 0x0, 0x2, 0xA2,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x4, 0x56, 0x0
        };

        

    }
}
