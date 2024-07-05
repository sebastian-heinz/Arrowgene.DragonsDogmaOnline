using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SDispelExchangeDispelItemReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_DISPEL_EXCHANGE_DISPEL_ITEM_REQ;

        public C2SDispelExchangeDispelItemReq()
        {
            GetDispelItemList = new List<CDataGetDispelItem>();
        }

        public List<CDataGetDispelItem> GetDispelItemList {  get; set; }

        public class Serializer : PacketEntitySerializer<C2SDispelExchangeDispelItemReq>
        {
            public override void Write(IBuffer buffer, C2SDispelExchangeDispelItemReq obj)
            {
                WriteEntityList(buffer, obj.GetDispelItemList);
            }

            public override C2SDispelExchangeDispelItemReq Read(IBuffer buffer)
            {
                C2SDispelExchangeDispelItemReq obj = new C2SDispelExchangeDispelItemReq();
                obj.GetDispelItemList = ReadEntityList<CDataGetDispelItem>(buffer);
                return obj;
            }
        }
    }
}

