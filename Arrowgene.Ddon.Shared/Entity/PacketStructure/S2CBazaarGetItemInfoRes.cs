using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBazaarGetItemInfoRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BAZAAR_GET_ITEM_INFO_RES;

        public S2CBazaarGetItemInfoRes()
        {
            BazaarItemList = new List<CDataBazaarItemInfo>();
        }

        public uint ItemId { get; set; }
        public List<CDataBazaarItemInfo> BazaarItemList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBazaarGetItemInfoRes>
        {
            public override void Write(IBuffer buffer, S2CBazaarGetItemInfoRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.ItemId);
                WriteEntityList<CDataBazaarItemInfo>(buffer, obj.BazaarItemList);
            }

            public override S2CBazaarGetItemInfoRes Read(IBuffer buffer)
            {
                S2CBazaarGetItemInfoRes obj = new S2CBazaarGetItemInfoRes();
                ReadServerResponse(buffer, obj);
                obj.ItemId = ReadUInt32(buffer);
                obj.BazaarItemList = ReadEntityList<CDataBazaarItemInfo>(buffer);
                return obj;
            }
        }
    }
}