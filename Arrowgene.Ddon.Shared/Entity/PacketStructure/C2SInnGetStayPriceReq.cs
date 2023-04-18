using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInnGetStayPriceReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INN_GET_STAY_PRICE_REQ;

        public uint InnId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SInnGetStayPriceReq>
        {
            public override void Write(IBuffer buffer, C2SInnGetStayPriceReq obj)
            {
                WriteUInt32(buffer, obj.InnId);
            }

            public override C2SInnGetStayPriceReq Read(IBuffer buffer)
            {
                C2SInnGetStayPriceReq obj = new C2SInnGetStayPriceReq();
                obj.InnId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
