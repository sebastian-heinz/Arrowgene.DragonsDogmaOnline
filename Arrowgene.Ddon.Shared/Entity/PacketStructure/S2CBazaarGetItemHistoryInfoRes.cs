using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBazaarGetItemHistoryInfoRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BAZAAR_GET_ITEM_HISTORY_INFO_RES;

        public S2CBazaarGetItemHistoryInfoRes()
        {
            BazaarItemHistoryList = new List<CDataBazaarItemHistoryInfo>();
        }

        public uint ItemId { get; set; }
        public List<CDataBazaarItemHistoryInfo> BazaarItemHistoryList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBazaarGetItemHistoryInfoRes>
        {
            public override void Write(IBuffer buffer, S2CBazaarGetItemHistoryInfoRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.ItemId);
                WriteEntityList<CDataBazaarItemHistoryInfo>(buffer, obj.BazaarItemHistoryList);
            }

            public override S2CBazaarGetItemHistoryInfoRes Read(IBuffer buffer)
            {
                S2CBazaarGetItemHistoryInfoRes obj = new S2CBazaarGetItemHistoryInfoRes();
                ReadServerResponse(buffer, obj);
                obj.ItemId = ReadUInt32(buffer);
                obj.BazaarItemHistoryList = ReadEntityList<CDataBazaarItemHistoryInfo>(buffer);
                return obj;
            }
        }
    }
}