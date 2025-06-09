using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobOrbTreeGetCurrencyExchangeRes : ServerResponse
    {
        public S2CJobOrbTreeGetCurrencyExchangeRes()
        {
        }
        public override PacketId Id => PacketId.S2C_JOB_ORB_TREE_GET_CURRENCY_EXCHANGE_LIST_RES;

        public List<CDataJobOrbCurrencyExchange> CurrencyExchangeList { get; set; } = new();
        public bool EnableOrbExchange { get; set; }

        public class Serializer : PacketEntitySerializer<S2CJobOrbTreeGetCurrencyExchangeRes>
        {
            public override void Write(IBuffer buffer, S2CJobOrbTreeGetCurrencyExchangeRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.CurrencyExchangeList);
                WriteBool(buffer, obj.EnableOrbExchange);
            }

            public override S2CJobOrbTreeGetCurrencyExchangeRes Read(IBuffer buffer)
            {
                S2CJobOrbTreeGetCurrencyExchangeRes obj = new S2CJobOrbTreeGetCurrencyExchangeRes();
                ReadServerResponse(buffer, obj);
                obj.CurrencyExchangeList = ReadEntityList<CDataJobOrbCurrencyExchange>(buffer);
                obj.EnableOrbExchange = ReadBool(buffer);
                return obj;
            }
        }
    }
}
