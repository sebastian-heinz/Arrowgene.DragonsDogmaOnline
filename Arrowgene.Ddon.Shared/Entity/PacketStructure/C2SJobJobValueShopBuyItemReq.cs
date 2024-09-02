using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SJobJobValueShopBuyItemReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_JOB_JOB_VALUE_SHOP_BUY_ITEM_REQ;

        public C2SJobJobValueShopBuyItemReq()
        {
        }

        public JobId JobId { get; set; }
        public byte JobValueType { get; set; }
        public uint LineupId { get; set; }
        public byte Num { get; set; }
        public byte StorageType { get; set; }
        public uint Price { get; set; }

        public class Serializer : PacketEntitySerializer<C2SJobJobValueShopBuyItemReq>
        {

            public override void Write(IBuffer buffer, C2SJobJobValueShopBuyItemReq obj)
            {
                WriteByte(buffer, (byte)obj.JobId);
                WriteByte(buffer, obj.JobValueType);
                WriteUInt32(buffer, obj.LineupId);
                WriteByte(buffer, obj.Num);
                WriteByte(buffer, obj.StorageType);
                WriteUInt32(buffer, obj.Price);
            }

            public override C2SJobJobValueShopBuyItemReq Read(IBuffer buffer)
            {
                C2SJobJobValueShopBuyItemReq obj = new C2SJobJobValueShopBuyItemReq();
                obj.JobId = (JobId)ReadByte(buffer);
                obj.JobValueType = ReadByte(buffer);
                obj.LineupId = ReadUInt32(buffer);
                obj.Num = ReadByte(buffer);
                obj.StorageType = ReadByte(buffer);
                obj.Price = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
