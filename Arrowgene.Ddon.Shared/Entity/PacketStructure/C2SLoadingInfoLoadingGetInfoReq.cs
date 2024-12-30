using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SLoadingInfoLoadingGetInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_LOADING_INFO_LOADING_GET_INFO_REQ;
        public class Serializer : PacketEntitySerializer<C2SLoadingInfoLoadingGetInfoReq>
        {
            public override void Write(IBuffer buffer, C2SLoadingInfoLoadingGetInfoReq obj)
            {
            }

            public override C2SLoadingInfoLoadingGetInfoReq Read(IBuffer buffer)
            {
                C2SLoadingInfoLoadingGetInfoReq obj = new C2SLoadingInfoLoadingGetInfoReq();
                return obj;
            }
        }
    }
}
