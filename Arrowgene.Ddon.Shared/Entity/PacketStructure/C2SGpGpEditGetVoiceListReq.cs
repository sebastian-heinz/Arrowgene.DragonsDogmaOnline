using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SGpGpEditGetVoiceListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_GP_GP_EDIT_GET_VOICE_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SGpGpEditGetVoiceListReq>
        {
            public override void Write(IBuffer buffer, C2SGpGpEditGetVoiceListReq obj)
            {
            }

            public override C2SGpGpEditGetVoiceListReq Read(IBuffer buffer)
            {
                C2SGpGpEditGetVoiceListReq obj = new C2SGpGpEditGetVoiceListReq();
                return obj;
            }
        }

    }
}