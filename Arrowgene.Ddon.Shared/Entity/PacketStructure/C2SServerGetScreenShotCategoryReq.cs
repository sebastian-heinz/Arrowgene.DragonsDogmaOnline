using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SServerGetScreenShotCategoryReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SERVER_GET_SCREEN_SHOT_CATEGORY_REQ;

        public class Serializer : PacketEntitySerializer<C2SServerGetScreenShotCategoryReq>
        {
            public override void Write(IBuffer buffer, C2SServerGetScreenShotCategoryReq obj)
            {
            }

            public override C2SServerGetScreenShotCategoryReq Read(IBuffer buffer)
            {
                C2SServerGetScreenShotCategoryReq obj = new C2SServerGetScreenShotCategoryReq();
                return obj;
            }
        }
    }
}
