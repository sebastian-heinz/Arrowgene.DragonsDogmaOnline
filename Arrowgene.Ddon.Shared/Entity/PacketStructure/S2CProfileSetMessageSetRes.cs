using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CProfileSetMessageSetRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PROFILE_SET_MESSAGE_SET_RES;

        public class Serializer : PacketEntitySerializer<S2CProfileSetMessageSetRes>
        {
            public override void Write(IBuffer buffer, S2CProfileSetMessageSetRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CProfileSetMessageSetRes Read(IBuffer buffer)
            {
                S2CProfileSetMessageSetRes obj = new S2CProfileSetMessageSetRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
