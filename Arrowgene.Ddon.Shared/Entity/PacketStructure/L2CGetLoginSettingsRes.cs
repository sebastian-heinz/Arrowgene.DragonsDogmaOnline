using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class L2CGetLoginSettingsRes : ServerResponse
    {
        public L2CGetLoginSettingsRes()
        {
            LoginSetting = new CDataLoginSetting();
        }

        public override PacketId Id => PacketId.L2C_GET_LOGIN_SETTING_RES;

        public CDataLoginSetting LoginSetting;

        public class Serializer : EntitySerializer<L2CGetLoginSettingsRes>
        {
            static Serializer()
            {
                Id = PacketId.L2C_GET_LOGIN_SETTING_RES;
            }
            
            public override void Write(IBuffer buffer, L2CGetLoginSettingsRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity(buffer, obj.LoginSetting);
            }

            public override L2CGetLoginSettingsRes Read(IBuffer buffer)
            {
                L2CGetLoginSettingsRes obj = new L2CGetLoginSettingsRes();
                ReadServerResponse(buffer, obj);
                obj.LoginSetting = ReadEntity<CDataLoginSetting>(buffer);
                return obj;
            }
        }
    }
}
