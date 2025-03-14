using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class L2CDecideCancelCharacterRes : ServerResponse
    {
        public override PacketId Id => PacketId.L2C_DECIDE_CANCEL_CHARACTER_RES;

        public class Serializer : PacketEntitySerializer<L2CDecideCancelCharacterRes>
        {
            public override void Write(IBuffer buffer, L2CDecideCancelCharacterRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override L2CDecideCancelCharacterRes Read(IBuffer buffer)
            {
                L2CDecideCancelCharacterRes obj = new L2CDecideCancelCharacterRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
