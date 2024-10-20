using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SServerGetGameSettingReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SERVER_GET_GAME_SETTING_REQ;

        public class Serializer : PacketEntitySerializer<C2SServerGetGameSettingReq>
        {

            public override void Write(IBuffer buffer, C2SServerGetGameSettingReq obj)
            {
            }

            public override C2SServerGetGameSettingReq Read(IBuffer buffer)
            {
                C2SServerGetGameSettingReq obj = new C2SServerGetGameSettingReq();
                return obj;
            }
        }
    }
}
