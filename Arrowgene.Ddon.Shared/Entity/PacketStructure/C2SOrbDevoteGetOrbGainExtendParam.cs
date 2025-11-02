using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SOrbDevoteGetOrbGainExtendParam : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ORB_DEVOTE_GET_ORB_GAIN_EXTEND_PARAM_REQ;

        public class Serializer : PacketEntitySerializer<C2SOrbDevoteGetOrbGainExtendParam>
        {
            public override void Write(IBuffer buffer, C2SOrbDevoteGetOrbGainExtendParam obj)
            {
            }

            public override C2SOrbDevoteGetOrbGainExtendParam Read(IBuffer buffer)
            {
                C2SOrbDevoteGetOrbGainExtendParam obj = new C2SOrbDevoteGetOrbGainExtendParam();
                return obj;
            }
        }
    }
}
