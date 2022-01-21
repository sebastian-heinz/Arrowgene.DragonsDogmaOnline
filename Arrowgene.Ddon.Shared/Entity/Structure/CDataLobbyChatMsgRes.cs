using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure {
    public class CDataLobbyChatMsgRes {
        // It has nothing
    }

    public class CDataLobbyChatMsgResSerializer : EntitySerializer<CDataLobbyChatMsgRes>
    {
        public override void Write(IBuffer buffer, CDataLobbyChatMsgRes obj)
        {
            // Do nothing
        }

        public override CDataLobbyChatMsgRes Read(IBuffer buffer)
        {
            CDataLobbyChatMsgRes obj = new CDataLobbyChatMsgRes();
            return obj;
        }
    }
}