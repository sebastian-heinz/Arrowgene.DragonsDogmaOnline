using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobOrbTreeExchangeCurrencyRes : ServerResponse
    {
        public S2CJobOrbTreeExchangeCurrencyRes()
        {
        }
        public override PacketId Id => PacketId.S2C_JOB_ORB_TREE_EXCHANGE_CURRENCY_RES;

        public uint WalletType0 { get; set; }
        public uint AmountType0 { get; set; }
        
        public uint WalletType1 { get; set; }
        public uint AmountType1 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CJobOrbTreeExchangeCurrencyRes>
        {
            public override void Write(IBuffer buffer, S2CJobOrbTreeExchangeCurrencyRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.WalletType0);
                WriteUInt32(buffer, obj.AmountType0);
                WriteUInt32(buffer, obj.WalletType1);
                WriteUInt32(buffer, obj.AmountType1);
            }

            public override S2CJobOrbTreeExchangeCurrencyRes Read(IBuffer buffer)
            {
                S2CJobOrbTreeExchangeCurrencyRes obj = new S2CJobOrbTreeExchangeCurrencyRes();
                ReadServerResponse(buffer, obj);
                obj.WalletType0 = ReadUInt32(buffer);
                obj.AmountType0 = ReadUInt32(buffer);
                obj.WalletType1 = ReadUInt32(buffer);
                obj.AmountType1 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
