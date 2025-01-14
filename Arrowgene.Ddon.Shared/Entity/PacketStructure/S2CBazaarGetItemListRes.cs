using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBazaarGetItemListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BAZAAR_GET_ITEM_LIST_RES;

        public S2CBazaarGetItemListRes()
        {
            ItemList = new List<CDataBazaarItemNumOfExhibitionInfo>();
            IgnoredItemIdList = new List<CDataCommonU32>();
        }

        public List<CDataBazaarItemNumOfExhibitionInfo> ItemList { get; set; }
        /// <summary>
        /// List of ItemIDs that are removed from the search result.
        /// </summary>
        public List<CDataCommonU32> IgnoredItemIdList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBazaarGetItemListRes>
        {
            public override void Write(IBuffer buffer, S2CBazaarGetItemListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataBazaarItemNumOfExhibitionInfo>(buffer, obj.ItemList);
                WriteEntityList<CDataCommonU32>(buffer, obj.IgnoredItemIdList);
            }

            public override S2CBazaarGetItemListRes Read(IBuffer buffer)
            {
                S2CBazaarGetItemListRes obj = new S2CBazaarGetItemListRes();
                ReadServerResponse(buffer, obj);
                obj.ItemList = ReadEntityList<CDataBazaarItemNumOfExhibitionInfo>(buffer);
                obj.IgnoredItemIdList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
