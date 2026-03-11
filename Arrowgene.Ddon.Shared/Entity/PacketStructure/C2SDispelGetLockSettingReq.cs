using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SDispelGetLockSettingReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_DISPEL_GET_LOCK_SETTING_REQ;

        public class Serializer : PacketEntitySerializer<C2SDispelGetLockSettingReq>
        {
            public override void Write(IBuffer buffer, C2SDispelGetLockSettingReq obj)
            {
            }

            public override C2SDispelGetLockSettingReq Read(IBuffer buffer)
            {
                C2SDispelGetLockSettingReq obj = new C2SDispelGetLockSettingReq();
                return obj;
            }
        }
    }
}
