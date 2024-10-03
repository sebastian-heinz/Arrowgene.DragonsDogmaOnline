using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CDispelExchangeDispelItemRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_DISPEL_EXCHANGE_DISPEL_ITEM_RES;

        public S2CDispelExchangeDispelItemRes()
        {
            DispelItemResultList = new List<CDataDispelResultInfo>();
        }

        public List<CDataDispelResultInfo> DispelItemResultList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CDispelExchangeDispelItemRes>
        {
            public override void Write(IBuffer buffer, S2CDispelExchangeDispelItemRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.DispelItemResultList);
            }

            public override S2CDispelExchangeDispelItemRes Read(IBuffer buffer)
            {
                S2CDispelExchangeDispelItemRes obj = new S2CDispelExchangeDispelItemRes();
                ReadServerResponse(buffer, obj);
                obj.DispelItemResultList = ReadEntityList<CDataDispelResultInfo>(buffer);
                return obj;
            }
        }
    }
}
