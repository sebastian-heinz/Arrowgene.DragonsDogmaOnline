using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure {
    public class S2CSetShortcutRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PROFILE_SET_SHORTCUT_LIST_RES;

        public class Serializer : PacketEntitySerializer<S2CSetShortcutRes> {
            public override void Write(IBuffer buffer, S2CSetShortcutRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CSetShortcutRes Read(IBuffer buffer)
            {
                S2CSetShortcutRes obj = new S2CSetShortcutRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }

    }

}
