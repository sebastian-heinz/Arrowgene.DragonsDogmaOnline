using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure {
    public class S2CSetCommunicationShortcutRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PROFILE_SET_COMMUNICATION_SHORTCUT_LIST_RES;


        public S2CSetCommunicationShortcutRes() {

        }

        public class Serializer : PacketEntitySerializer<S2CSetCommunicationShortcutRes> {
            public override void Write(IBuffer buffer, S2CSetCommunicationShortcutRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CSetCommunicationShortcutRes Read(IBuffer buffer)
            {
                S2CSetCommunicationShortcutRes obj = new S2CSetCommunicationShortcutRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }

    }

}
