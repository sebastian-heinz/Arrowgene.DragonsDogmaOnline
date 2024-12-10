using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CChatSendTellMsgRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CHAT_SEND_TELL_MSG_RES;

        public S2CChatSendTellMsgRes()
        {
            CharacterBaseInfo = new();
        }

        public CDataCommunityCharacterBaseInfo CharacterBaseInfo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CChatSendTellMsgRes>
        {
            public override void Write(IBuffer buffer, S2CChatSendTellMsgRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteEntity<CDataCommunityCharacterBaseInfo>(buffer, obj.CharacterBaseInfo);
            }

            public override S2CChatSendTellMsgRes Read(IBuffer buffer)
            {
                S2CChatSendTellMsgRes obj = new S2CChatSendTellMsgRes();

                ReadServerResponse(buffer, obj);

                obj.CharacterBaseInfo = ReadEntity<CDataCommunityCharacterBaseInfo>(buffer);

                return obj;
            }
        }
    }
}
