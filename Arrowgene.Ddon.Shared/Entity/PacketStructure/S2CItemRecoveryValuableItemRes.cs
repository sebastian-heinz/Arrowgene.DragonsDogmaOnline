using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CItemRecoveryValuableItemRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ITEM_RECOVERY_VALUABLE_ITEM_RES;

        public class Serializer : PacketEntitySerializer<S2CItemRecoveryValuableItemRes>
        {
            public override void Write(IBuffer buffer, S2CItemRecoveryValuableItemRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CItemRecoveryValuableItemRes Read(IBuffer buffer)
            {
                S2CItemRecoveryValuableItemRes obj = new S2CItemRecoveryValuableItemRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
