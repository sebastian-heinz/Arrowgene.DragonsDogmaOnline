using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CDispelLockSettingRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_DISPEL_LOCK_SETTING_RES;

        public List<CDataDispelLockSettingUpdate> Unk0 { get; set; } = [];

        public class Serializer : PacketEntitySerializer<S2CDispelLockSettingRes>
        {
            public override void Write(IBuffer buffer, S2CDispelLockSettingRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.Unk0);
            }

            public override S2CDispelLockSettingRes Read(IBuffer buffer)
            {
                S2CDispelLockSettingRes obj = new S2CDispelLockSettingRes();
                ReadServerResponse(buffer, obj);
                obj.Unk0 = ReadEntityList<CDataDispelLockSettingUpdate>(buffer);
                return obj;
            }
        }
    }
}
