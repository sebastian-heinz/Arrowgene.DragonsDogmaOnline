using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SAreaGetAreaRewardInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_AREA_GET_AREA_REWARD_INFO_REQ;

        public class Serializer : PacketEntitySerializer<C2SAreaGetAreaRewardInfoReq>
        {
            public override void Write(IBuffer buffer, C2SAreaGetAreaRewardInfoReq obj)
            {
                
            }

            public override C2SAreaGetAreaRewardInfoReq Read(IBuffer buffer)
            {
                C2SAreaGetAreaRewardInfoReq obj = new C2SAreaGetAreaRewardInfoReq();
                return obj;
            }
        }
    }
}
