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
    public class S2CJobJobValueShopBuyItemRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_JOB_JOB_VALUE_SHOP_BUY_ITEM_RES;

        public S2CJobJobValueShopBuyItemRes()
        {
            JobId = 0;
            JobValueType = 0;
            Value = 0;
        }

        public JobId JobId { get; set; }
        public byte JobValueType { get; set; }
        public uint Value { get; set; }

        public class Serializer : PacketEntitySerializer<S2CJobJobValueShopBuyItemRes>
        {
            public override void Write(IBuffer buffer, S2CJobJobValueShopBuyItemRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, (byte)obj.JobId);
                WriteByte(buffer, obj.JobValueType);
                WriteUInt32(buffer, obj.Value);
            }

            public override S2CJobJobValueShopBuyItemRes Read(IBuffer buffer)
            {
                S2CJobJobValueShopBuyItemRes obj = new S2CJobJobValueShopBuyItemRes();
                ReadServerResponse(buffer, obj);
                obj.JobId = (JobId)ReadByte(buffer);
                obj.JobValueType = ReadByte(buffer);
                obj.Value = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
