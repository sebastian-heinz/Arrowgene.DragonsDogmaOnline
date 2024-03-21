using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBazaarGetItemListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BAZAAR_GET_ITEM_LIST_REQ;

        public C2SBazaarGetItemListReq()
        {
            ItemIdList = new List<CDataCommonU32>();
        }

        public List<CDataCommonU32> ItemIdList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SBazaarGetItemListReq>
        {
            public override void Write(IBuffer buffer, C2SBazaarGetItemListReq obj)
            {
                WriteEntityList<CDataCommonU32>(buffer, obj.ItemIdList);
            }

            public override C2SBazaarGetItemListReq Read(IBuffer buffer)
            {
                C2SBazaarGetItemListReq obj = new C2SBazaarGetItemListReq();
                obj.ItemIdList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }

    }
}