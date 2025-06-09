using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobOrbCurrencyExchange
    {
        public CDataJobOrbCurrencyExchange()
        {
        }

        public uint Unk0 { get; set; } // Value is passed on to unk0 of C2SJobOrbTreeExchangeCurrencyReq
        public uint WalletTypeTo { get; set; }
        public uint WalletTypeFrom { get; set; }
        public uint ConversionRate { get; set; }
        public uint Unk4 { get; set; }

        public class Serializer : EntitySerializer<CDataJobOrbCurrencyExchange>
        {
            public override void Write(IBuffer buffer, CDataJobOrbCurrencyExchange obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.WalletTypeTo);
                WriteUInt32(buffer, obj.WalletTypeFrom);
                WriteUInt32(buffer, obj.ConversionRate);
                WriteUInt32(buffer, obj.Unk4);
            }

            public override CDataJobOrbCurrencyExchange Read(IBuffer buffer)
            {
                CDataJobOrbCurrencyExchange obj = new CDataJobOrbCurrencyExchange();
                obj.Unk0 = ReadUInt32(buffer);
                obj.WalletTypeTo = ReadUInt32(buffer);
                obj.WalletTypeFrom = ReadUInt32(buffer);
                obj.ConversionRate = ReadUInt32(buffer);
                obj.Unk4 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
