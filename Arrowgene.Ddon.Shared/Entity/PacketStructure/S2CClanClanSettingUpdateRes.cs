using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanSettingUpdateRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_SETTING_UPDATE_RES;

        public S2CClanClanSettingUpdateRes()
        {
        }

        public bool IsMemberNotice;

        public class Serializer : PacketEntitySerializer<S2CClanClanSettingUpdateRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanSettingUpdateRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteBool(buffer, obj.IsMemberNotice);
            }

            public override S2CClanClanSettingUpdateRes Read(IBuffer buffer)
            {
                S2CClanClanSettingUpdateRes obj = new S2CClanClanSettingUpdateRes();
                ReadServerResponse(buffer, obj);
                obj.IsMemberNotice = ReadBool(buffer);
                return obj;
            }
        }
    }
}
