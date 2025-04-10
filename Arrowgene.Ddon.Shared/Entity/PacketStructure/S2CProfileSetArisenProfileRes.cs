using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CProfileSetArisenProfileRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PROFILE_SET_ARISEN_PROFILE_RES;

        public class Serializer : PacketEntitySerializer<S2CProfileSetArisenProfileRes>
        {
            public override void Write(IBuffer buffer, S2CProfileSetArisenProfileRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CProfileSetArisenProfileRes Read(IBuffer buffer)
            {
                S2CProfileSetArisenProfileRes obj = new S2CProfileSetArisenProfileRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
