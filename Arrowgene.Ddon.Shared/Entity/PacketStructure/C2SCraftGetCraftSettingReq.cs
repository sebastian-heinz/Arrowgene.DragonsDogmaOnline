using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftGetCraftSettingReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CRAFT_GET_CRAFT_SETTING_REQ;

        public class Serializer : PacketEntitySerializer<C2SCraftGetCraftSettingReq>
        {
            public override void Write(IBuffer buffer, C2SCraftGetCraftSettingReq obj)
            {
            }

            public override C2SCraftGetCraftSettingReq Read(IBuffer buffer)
            {
                C2SCraftGetCraftSettingReq obj = new C2SCraftGetCraftSettingReq();
                return obj;
            }
        }

    }
}