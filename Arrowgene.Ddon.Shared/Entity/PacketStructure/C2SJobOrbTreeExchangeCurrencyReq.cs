using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SJobOrbTreeExchangeCurrencyReq : IPacketStructure
    {
        public C2SJobOrbTreeExchangeCurrencyReq()
        {
        }
        public PacketId Id => PacketId.C2S_JOB_ORB_TREE_EXCHANGE_CURRENCY_REQ;

        public uint Unk0 { get; set; }
        public uint ExchangeAmount { get; set; }

        public class Serializer : PacketEntitySerializer<C2SJobOrbTreeExchangeCurrencyReq>
        {
            public override void Write(IBuffer buffer, C2SJobOrbTreeExchangeCurrencyReq obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.ExchangeAmount);
            }

            public override C2SJobOrbTreeExchangeCurrencyReq Read(IBuffer buffer)
            {
                C2SJobOrbTreeExchangeCurrencyReq obj = new C2SJobOrbTreeExchangeCurrencyReq();
                obj.Unk0 = ReadUInt32(buffer);
                obj.ExchangeAmount = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

