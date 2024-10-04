using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanSettingUpdateReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_SETTING_UPDATE_REQ;

        public C2SClanClanSettingUpdateReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SClanClanSettingUpdateReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanSettingUpdateReq obj)
            {
            }

            public override C2SClanClanSettingUpdateReq Read(IBuffer buffer)
            {
                C2SClanClanSettingUpdateReq obj = new C2SClanClanSettingUpdateReq();
                return obj;
            }
        }
    }
}
