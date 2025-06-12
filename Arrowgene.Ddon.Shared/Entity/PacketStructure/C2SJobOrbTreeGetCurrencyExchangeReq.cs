using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SJobOrbTreeGetCurrencyExchangeReq : IPacketStructure
    {
        public C2SJobOrbTreeGetCurrencyExchangeReq()
        {
        }
        public PacketId Id => PacketId.C2S_JOB_ORB_TREE_GET_CURRENCY_EXCHANGE_LIST_REQ;

        public uint Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<C2SJobOrbTreeGetCurrencyExchangeReq>
        {
            public override void Write(IBuffer buffer, C2SJobOrbTreeGetCurrencyExchangeReq obj)
            {
                WriteUInt32(buffer, obj.Unk0);
            }

            public override C2SJobOrbTreeGetCurrencyExchangeReq Read(IBuffer buffer)
            {
                C2SJobOrbTreeGetCurrencyExchangeReq obj = new C2SJobOrbTreeGetCurrencyExchangeReq();
                obj.Unk0 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

