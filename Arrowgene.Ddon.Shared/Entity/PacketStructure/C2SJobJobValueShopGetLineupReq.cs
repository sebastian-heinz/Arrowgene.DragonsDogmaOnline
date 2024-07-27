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
    public class C2SJobJobValueShopGetLineupReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_JOB_JOB_VALUE_SHOP_GET_LINEUP_REQ;

        public C2SJobJobValueShopGetLineupReq()
        {
            JobId = 0;
            JobValueType = 0;
        }

        public JobId JobId { get; set; }
        public byte JobValueType { get; set; }

        public class Serializer : PacketEntitySerializer<C2SJobJobValueShopGetLineupReq>
        {

            public override void Write(IBuffer buffer, C2SJobJobValueShopGetLineupReq obj)
            {
                WriteByte(buffer, (byte)obj.JobId);
                WriteByte(buffer, obj.JobValueType);
            }

            public override C2SJobJobValueShopGetLineupReq Read(IBuffer buffer)
            {
                C2SJobJobValueShopGetLineupReq obj = new C2SJobJobValueShopGetLineupReq();
                obj.JobId = (JobId)ReadByte(buffer);
                obj.JobValueType = ReadByte(buffer);
                return obj;
            }
        }
    }
}
