using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class L2CGetErrorMessageListRes : ServerResponse
    {
        public override PacketId Id => PacketId.L2C_GET_ERROR_MESSAGE_LIST_RES;

        public class Serializer : PacketEntitySerializer<L2CGetErrorMessageListRes>
        {

            public override void Write(IBuffer buffer, L2CGetErrorMessageListRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override L2CGetErrorMessageListRes Read(IBuffer buffer)
            {
                L2CGetErrorMessageListRes obj = new L2CGetErrorMessageListRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
