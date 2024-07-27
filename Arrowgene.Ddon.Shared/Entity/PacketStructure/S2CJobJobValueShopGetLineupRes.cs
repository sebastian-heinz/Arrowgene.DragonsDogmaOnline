using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobJobValueShopGetLineupRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_JOB_JOB_VALUE_SHOP_GET_LINEUP_RES;

        public S2CJobJobValueShopGetLineupRes()
        {
            JobId = 0;
            JobValueType = 0;
            JobValueShopItemList = new List<CDataJobValueShopItem>();
        }

        public JobId JobId { get; set; }
        public byte JobValueType { get; set; }
        public List<CDataJobValueShopItem> JobValueShopItemList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CJobJobValueShopGetLineupRes>
        {
            public override void Write(IBuffer buffer, S2CJobJobValueShopGetLineupRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, (byte)obj.JobId);
                WriteByte(buffer, obj.JobValueType);
                WriteEntityList<CDataJobValueShopItem>(buffer, obj.JobValueShopItemList);
            }

            public override S2CJobJobValueShopGetLineupRes Read(IBuffer buffer)
            {
                S2CJobJobValueShopGetLineupRes obj = new S2CJobJobValueShopGetLineupRes();
                ReadServerResponse(buffer, obj);
                obj.JobId = (JobId)ReadByte(buffer);
                obj.JobValueType = ReadByte(buffer);
                obj.JobValueShopItemList = ReadEntityList<CDataJobValueShopItem>(buffer);
                return obj;
            }
        }
    }
}
