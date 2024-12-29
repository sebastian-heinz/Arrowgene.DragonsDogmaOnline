using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2LGetLoginSettingReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2L_GET_LOGIN_SETTING_REQ;

        public class Serializer : PacketEntitySerializer<C2LGetLoginSettingReq>
        {

            public override void Write(IBuffer buffer, C2LGetLoginSettingReq obj)
            {
            }

            public override C2LGetLoginSettingReq Read(IBuffer buffer)
            {
                C2LGetLoginSettingReq obj = new C2LGetLoginSettingReq();
                return obj;
            }
        }
    }
}
