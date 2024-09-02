using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftCancelCraftRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CRAFT_CANCEL_CRAFT_RES;

        public class Serializer : PacketEntitySerializer<C2SCraftCancelCraftRes>
        {
            public override void Write(IBuffer buffer, C2SCraftCancelCraftRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override C2SCraftCancelCraftRes Read(IBuffer buffer)
            {
                C2SCraftCancelCraftRes obj = new C2SCraftCancelCraftRes();

                ReadServerResponse(buffer, obj);

                return obj;
            }
        }
    }
}
