using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure {
    public class S2CLobbyChatMsgRes : ServerResponse {
        // This response has no data
        public override PacketId Id => PacketId.S2C_LOBBY_LOBBY_CHAT_MSG_RES;

        public class Serializer : PacketEntitySerializer<S2CLobbyChatMsgRes>
        {

            public override void Write(IBuffer buffer, S2CLobbyChatMsgRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CLobbyChatMsgRes Read(IBuffer buffer)
            {
                S2CLobbyChatMsgRes obj = new S2CLobbyChatMsgRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }

}
