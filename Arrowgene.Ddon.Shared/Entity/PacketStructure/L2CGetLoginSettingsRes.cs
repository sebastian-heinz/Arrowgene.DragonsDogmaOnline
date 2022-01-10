using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class L2CGetLoginSettingsRes
    {
        public L2CGetLoginSettingsRes()
        {
            LoginSetting = new CDataLoginSetting();
        }

        public CDataLoginSetting LoginSetting;
    }

    public class L2CGetLoginSettingsResSerializer : EntitySerializer<L2CGetLoginSettingsRes>
    {
        public override void Write(IBuffer buffer, L2CGetLoginSettingsRes obj)
        {
            WriteEntity(buffer, obj.LoginSetting);
        }

        public override L2CGetLoginSettingsRes Read(IBuffer buffer)
        {
            L2CGetLoginSettingsRes obj = new L2CGetLoginSettingsRes();
            obj.LoginSetting = ReadEntity<CDataLoginSetting>(buffer);
            return obj;
        }
    }
}
