using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure {
    public class S2CLobbyChatMsgRes {
        // It has nothing
    }

    public class S2CLobbyChatMsgResSerializer : EntitySerializer<S2CLobbyChatMsgRes>
    {
        public override void Write(IBuffer buffer, S2CLobbyChatMsgRes obj)
        {
            // Do nothing
        }

        public override S2CLobbyChatMsgRes Read(IBuffer buffer)
        {
            S2CLobbyChatMsgRes obj = new S2CLobbyChatMsgRes();
            return obj;
        }
    }
}