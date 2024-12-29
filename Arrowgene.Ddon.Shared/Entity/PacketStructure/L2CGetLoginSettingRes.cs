using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class L2CGetLoginSettingRes : ServerResponse
    {
        public L2CGetLoginSettingRes()
        {
            LoginSetting = new CDataLoginSetting();
        }

        public override PacketId Id => PacketId.L2C_GET_LOGIN_SETTING_RES;

        public CDataLoginSetting LoginSetting { get; set; }

        public class Serializer : PacketEntitySerializer<L2CGetLoginSettingRes>
        {
            public override void Write(IBuffer buffer, L2CGetLoginSettingRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity(buffer, obj.LoginSetting);
            }

            public override L2CGetLoginSettingRes Read(IBuffer buffer)
            {
                L2CGetLoginSettingRes obj = new L2CGetLoginSettingRes();
                ReadServerResponse(buffer, obj);
                obj.LoginSetting = ReadEntity<CDataLoginSetting>(buffer);
                return obj;
            }
        }
    }
}
