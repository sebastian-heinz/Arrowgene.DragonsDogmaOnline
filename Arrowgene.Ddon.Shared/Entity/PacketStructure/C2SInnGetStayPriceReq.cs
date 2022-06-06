using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInnGetStayPriceReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INN_GET_STAY_PRICE_REQ;

        public uint Data0 { get; set; }
        public byte Data1 { get; set; }
        public byte Data2 { get; set; }
        public byte Data3 { get; set; }

        public C2SInnGetStayPriceReq()
        {
            Data0 = 0;
            Data1 = 0;
            Data2 = 0;
            Data3 = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SInnGetStayPriceReq>
        {
            public override void Write(IBuffer buffer, C2SInnGetStayPriceReq obj)
            {
                WriteUInt32(buffer, obj.Data0);
                WriteByte(buffer, obj.Data1);
                WriteByte(buffer, obj.Data2);
                WriteByte(buffer, obj.Data3);
            }

            public override C2SInnGetStayPriceReq Read(IBuffer buffer)
            {
                C2SInnGetStayPriceReq obj = new C2SInnGetStayPriceReq();
                obj.Data0 = ReadUInt32(buffer);
                obj.Data1 = ReadByte(buffer);
                obj.Data2 = ReadByte(buffer);
                obj.Data3 = ReadByte(buffer);
                return obj;
            }
        }
    }
}
